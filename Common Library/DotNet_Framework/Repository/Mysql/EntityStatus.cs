using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql
{
    public class EntityStatus
    {
        private List<ValidationResult> _errors = new List<ValidationResult>();

        /// <summary>
        /// If there are no errors then it is valid
        /// </summary>
        public bool IsValid 
        { 
            get { return _errors == null; } 
        }

        public IReadOnlyList<ValidationResult> Errors
        {

            get { return _errors ?? new List<ValidationResult>(); }
        }

        public EntityStatus(DbEntityValidationException ex)
        {
            SetErrors(ex);
        }

        public EntityStatus(DbUpdateException ex)
        {
            SetErrors(ex);
        }

        public EntityStatus(MySqlException ex)
        {
            SetErrors(ex);
        }

        public EntityStatus(Exception ex)
        {
            SetErrors(ex);
        }

        /// <summary>
        /// This converts the Entity framework errors into Validation errors
        /// </summary>
        private EntityStatus SetErrors(DbEntityValidationException error)
        {
            _errors = error.EntityValidationErrors.SelectMany(x =>
                        x.ValidationErrors.Select(y =>
                            new ValidationResult(y.ErrorMessage, new[] { y.PropertyName })))
                    .ToList();

            return this;
        }

        private EntityStatus SetErrors(DbUpdateException error)
        {
            Exception lastInnerException = error.InnerException;

            while (lastInnerException != null)
            {
                if (lastInnerException is MySqlException mysqlException)
                {
                    SetErrors(mysqlException);
                }
                else
                {
                    SetErrors(lastInnerException);
                }

                lastInnerException = lastInnerException.InnerException;
            }

            return this;
        }

        private EntityStatus SetErrors(MySqlException error)
        {
            _errors.Add(new ValidationResult(error.Message, new[] { $"LineNumber: {error.Number}" }));

            return this;
        }

        private EntityStatus SetErrors(Exception error)
        {
            _errors.Add(new ValidationResult(error.Message));

            return this;
        }
    }
}
