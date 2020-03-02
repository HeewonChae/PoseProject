using System;
using System.Configuration;
using System.ServiceModel.Configuration;

namespace SportsWebService.WebBehavior.Server.ServiceInitialize
{
    public class ServiceInitializationBehaviorSection : BehaviorExtensionElement
    {
        private readonly Lazy<ConfigurationPropertyCollection> _properties =
            new Lazy<ConfigurationPropertyCollection>(() =>
            {
                return new ConfigurationPropertyCollection
                {
                    new ConfigurationProperty("type",
                                              typeof(string),
                                              String.Empty,
                                              null,
                                              new StringValidator(Int32.MinValue,
                                                                  Int32.MaxValue,
                                                                  null),
                                              ConfigurationPropertyOptions.IsRequired)
                };
            });

        public override Type BehaviorType
        {
            get { return typeof(ServiceInitializationBehavior); }
        }

        [StringValidator(MinLength = Int32.MinValue, MaxLength = Int32.MaxValue)]
        public string InitializationType
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties.Value; }
        }

        public override void CopyFrom(ServiceModelExtensionElement from)
        {
            base.CopyFrom(from);

            var element = (ServiceInitializationBehaviorSection)from;

            InitializationType = element.InitializationType;
        }

        protected override object CreateBehavior()
        {
            var type = Type.GetType(InitializationType);
            if (type == null)
                throw new InvalidOperationException
                    ("Can't get read type from initialization type given");

            var serviceInitialization = Activator.CreateInstance(type) as IServiceInitialization;
            if (serviceInitialization == null)
                throw new InvalidOperationException
                    ("Can't create object from initialization type given");

            return new ServiceInitializationBehavior(serviceInitialization);
        }
    }
}