using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Fintrak.Shared.Common.Utils;
using FluentValidation;
using FluentValidation.Results;

namespace Fintrak.Shared.Common.Core
{
    public abstract class ObjectBase : NotificationObject, IDirtyCapable, IExtensibleDataObject, IDataErrorInfo
    {
        public ObjectBase()
        {
            _Validator = GetValidator();
            Validate();
        }

        protected bool _IsDirty = false;
        protected IValidator _Validator = null;

        bool _Deleted;
        string _CreatedBy;
        DateTime _CreatedOn;
        string _UpdatedBy;
        DateTime _UpdatedOn;
        byte[] _RowVersion;

        protected IEnumerable<ValidationFailure> _ValidationErrors = null;

        public static CompositionContainer? Container { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject? ExtensionData { get; set; }

        #endregion

        #region IDirtyCapable members

        [NotNavigable]
        public virtual bool IsDirty
        {
            get => _IsDirty;
            protected set
            {
                _IsDirty = value;
                OnPropertyChanged(nameof(IsDirty), false);
            }
        }

        public virtual bool IsAnythingDirty()
        {
            bool isDirty = false;

            WalkObjectGraph(
                o =>
                {
                    if (o.IsDirty)
                    {
                        isDirty = true;
                        return true; // short circuit
                    }
                    else
                        return false;
                }, coll => { });

            return isDirty;
        }

        public List<IDirtyCapable> GetDirtyObjects()
        {
            var dirtyObjects = new List<IDirtyCapable>();

            WalkObjectGraph(
                o =>
                {
                    if (o.IsDirty)
                        dirtyObjects.Add(o);

                    return false;
                }, coll => { });

            return dirtyObjects;
        }

        public void CleanAll()
        {
            WalkObjectGraph(
                o =>
                {
                    if (o.IsDirty)
                        o.IsDirty = false;
                    return false;
                }, coll => { });
        }

        #endregion

        #region Protected methods

        protected void WalkObjectGraph(Func<ObjectBase, bool> snippetForObject,
                                       Action<IList> snippetForCollection,
                                       params string[] exemptProperties)
        {
            var visited = new List<ObjectBase>();
            Action<ObjectBase> walk = null;

            var exemptions = exemptProperties?.ToList() ?? new List<string>();

            walk = o =>
            {
                if (o != null && !visited.Contains(o))
                {
                    visited.Add(o);

                    bool exitWalk = snippetForObject.Invoke(o);

                    if (!exitWalk)
                    {
                        var properties = o.GetBrowsableProperties();
                        foreach (var property in properties)
                        {
                            if (!exemptions.Contains(property.Name))
                            {
                                if (property.PropertyType.IsSubclassOf(typeof(ObjectBase)))
                                {
                                    var obj = (ObjectBase)(property.GetValue(o, null));
                                    walk(obj);
                                }
                                else
                                {
                                    var coll = property.GetValue(o, null) as IList;
                                    if (coll != null)
                                    {
                                        snippetForCollection.Invoke(coll);

                                        foreach (var item in coll)
                                        {
                                            if (item is ObjectBase)
                                                walk((ObjectBase)item);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            walk(this);
        }

        private PropertyInfo[] GetBrowsableProperties()
        {
            return this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => TypeDescriptor.GetProperties(this)[p.Name].IsBrowsable)
                .ToArray();
        }

        #endregion

        #region Property change notification

        protected override void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName, true);
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression, bool makeDirty)
        {
            var propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
            OnPropertyChanged(propertyName, makeDirty);
        }

        protected void OnPropertyChanged(string propertyName, bool makeDirty)
        {
            base.OnPropertyChanged(propertyName);

            if (makeDirty)
                IsDirty = true;

            Validate();
        }

        #endregion

        #region Validation

        protected virtual IValidator GetValidator()
        {
            return null;
        }

        [NotNavigable]
        public IEnumerable<ValidationFailure>? ValidationErrors
        {
            get => _ValidationErrors;
            set { }
        }

        public void Validate()
        {
            if (_Validator != null)
            {
                var results = _Validator.Validate((IValidationContext)this);
                _ValidationErrors = results.Errors;
            }
        }

        [NotNavigable]
        public virtual bool IsValid
        {
            get => _ValidationErrors == null || !_ValidationErrors.Any();
        }

        #endregion

        #region IDataErrorInfo members

        string IDataErrorInfo.Error => string.Empty;

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                var errors = new StringBuilder();

                if (_ValidationErrors != null && _ValidationErrors.Any())
                {
                    foreach (var validationError in _ValidationErrors)
                    {
                        if (validationError.PropertyName == columnName)
                            errors.AppendLine(validationError.ErrorMessage);
                    }
                }

                return errors.ToString();
            }
        }

        #endregion

        #region Audit

        public bool Deleted
        {
            get => _Deleted;
            set
            {
                if (_Deleted != value)
                {
                    _Deleted = value;
                    OnPropertyChanged(() => Deleted);
                }
            }
        }

        public string CreatedBy
        {
            get => _CreatedBy;
            set
            {
                if (_CreatedBy != value)
                {
                    _CreatedBy = value;
                    OnPropertyChanged(() => CreatedBy);
                }
            }
        }

        public DateTime CreatedOn
        {
            get => _CreatedOn;
            set
            {
                if (_CreatedOn != value)
                {
                    _CreatedOn = value;
                    OnPropertyChanged(() => CreatedOn);
                }
            }
        }

        public string UpdatedBy
        {
            get => _UpdatedBy;
            set
            {
                if (_UpdatedBy != value)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged(() => UpdatedBy);
                }
            }
        }

        public DateTime UpdatedOn
        {
            get => _UpdatedOn;
            set
            {
                if (_UpdatedOn != value)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged(() => UpdatedOn);
                }
            }
        }

        public byte[] RowVersion
        {
            get => _RowVersion;
            set
            {
                if (_RowVersion != value)
                {
                    _RowVersion = value;
                    OnPropertyChanged(() => RowVersion);
                }
            }
        }

        #endregion
    }
}
