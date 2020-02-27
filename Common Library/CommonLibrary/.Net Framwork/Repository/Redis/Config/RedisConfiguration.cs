using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Redis.Config
{
	public class RedisHost : ConfigurationElement
	{
		[ConfigurationProperty("host", IsRequired = true, IsKey = true)]
		public string Host
		{
			get { return (string)base["host"]; }
			set { base["host"] = value; }
		}
	}

	public class RedisHostCollection : System.Configuration.ConfigurationElementCollection
	{
		public RedisHost this[int index]
		{
			get
			{
				return base.BaseGet(index) as RedisHost;
			}
			set
			{
				if (base.BaseGet(index) != null)
				{
					base.BaseRemoveAt(index);
				}
				this.BaseAdd(index, value);
			}
		}
		protected
			override System.Configuration.ConfigurationElement CreateNewElement()
		{
			return new RedisHost();
		}

		protected override object GetElementKey(
			System.Configuration.ConfigurationElement element)
		{
			return ((RedisHost)element).Host;
		}
	}

	public class RedisConfigurationSection : ConfigurationSection
	{
		[ConfigurationProperty("abortConnect", IsRequired = false, DefaultValue = false)]
		public bool abortConnect
		{
			get { return (bool)base["abortConnect"]; }
			set { base["abortConnect"] = value; }
		}
		[ConfigurationProperty("allowAdmin", IsRequired = false, DefaultValue = false)]
		public bool allowAdmin
		{
			get { return (bool)base["allowAdmin"]; }
			set { base["allowAdmin"] = value; }
		}

		[ConfigurationProperty("connectRetry", IsRequired = false, DefaultValue = 3)]
		public int connectRetry
		{
			get { return (int)base["connectRetry"]; }
			set { base["connectRetry"] = value; }
		}

		[ConfigurationProperty("connectTimeout", IsRequired = false, DefaultValue = 5000)]
		public int connectTimeout
		{
			get { return (int)base["connectTimeout"]; }
			set { base["connectTimeout"] = value; }
		}

		[ConfigurationProperty("defaultDatabase", IsRequired = false, DefaultValue = null)]
		public int defaultDatabase
		{
			get { return (int)base["defaultDatabase"]; }
			set { base["defaultDatabase"] = value; }
		}

		[ConfigurationProperty("keepAlive", IsRequired = false, DefaultValue = 60)]
		public int keepAlive
		{
			get { return (int)base["keepAlive"]; }
			set { base["keepAlive"] = value; }
		}

		[ConfigurationProperty("name", IsRequired = false, DefaultValue = null)]
		public string name
		{
			get { return (string)base["name"]; }
			set { base["name"] = value; }
		}
		[ConfigurationProperty("password", IsRequired = false, DefaultValue = null)]
		public string password
		{
			get { return (string)base["password"]; }
			set { base["password"] = value; }
		}

		[ConfigurationProperty("syncTimeout", IsRequired = false, DefaultValue = 5000)]
		public int syncTimeout
		{
			get { return (int)base["syncTimeout"]; }
			set { base["syncTimeout"] = value; }
		}

		[ConfigurationProperty("resolveDns", IsRequired = false, DefaultValue = false)]
		public bool resolveDns
		{
			get { return (bool)base["resolveDns"]; }
			set { base["resolveDns"] = value; }
		}


		[System.Configuration.ConfigurationProperty("hosts")]
		public RedisHostCollection hosts
		{
			get
			{
				return (RedisHostCollection)this["hosts"] ??
				   new RedisHostCollection();
			}
		}

		public string ConnectionString
		{
			get
			{
				var hosts = (RedisHostCollection)this["hosts"] ?? new RedisHostCollection();
				var endPoints = new List<string>();
				foreach (RedisHost host in hosts)
				{
					endPoints.Add(host.Host);
				}
				return
					$@"{string.Join(",", endPoints.ToArray())}"
					+ $@",abortConnect={abortConnect}"
					+ $@",allowAdmin={allowAdmin}"
					+ $@",connectRetry={connectRetry}"
					+ $@",connectTimeout={connectTimeout}"
					+ $@",defaultDatabase={defaultDatabase}"
					+ $@",keepAlive={keepAlive}"
					+ $@",name={name}"
					+ $@",password={password}"
					+ $@",syncTimeout={syncTimeout}"
					+ $@",configCheckSeconds={10}"
					+ $@",resolveDns={resolveDns}"
					;
			}
		}
		public RedisConfigurationSection()
		{
		}
	}
}
