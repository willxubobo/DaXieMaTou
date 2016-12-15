1. 用法：
	参考如下的用法。
	注意：所有需要被缓存的对象，需要加上[Serializable]序列化注解

	static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            MemcachedCache mc = new MemcachedCache();

            string key = "key101";
            string value = "data101";
            mc.Put(key,value);
            object data = mc.Get(key);
            if (data != null)
            {
                Console.Out.WriteLine(data.ToString());
            }

            User user = new User();
            user.Name = "name1";
            user.Age = 30;

            key = "user";
            mc.Put(key, user);

            User userData = mc.Get<User>(key);
            if (userData != null)
            {
                Console.Out.WriteLine(userData.Name);
                Console.Out.WriteLine(userData.Age);
            }

            Console.ReadLine();
        }
    }

    [Serializable]
    public class User{
        public string Name { get; set; }
        public int Age { get; set; }
    }

2. 需要增加的配置文件：
	注意：memcached需要log4net的支持，所以在配置文件中需要增加log4net的配置项，项目引用中需要增加log4net的DLL引用
	<configuration>  
	  <configSections>
		<section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />

		<sectionGroup name="enyim.com">
		  <section name="memcached" type="Enyim.Caching.Configuration.MemcachedClientSection, Enyim.Caching"/>
		  <section name="log" type="Enyim.Caching.Configuration.LoggerSection, Enyim.Caching"/>
		</sectionGroup>
    
		<section name="memcached" type="Enyim.Caching.Configuration.MemcachedClientSection, Enyim.Caching"/>
    
	  </configSections>

	  <!-- This section contains the log4net configuration settings -->
	  <log4net>
		<root>
		  <level value="ALL"/>
		  <appender-ref ref="RollingLogFileAppender"/>
		</root>
		<logger name="Enyim.Caching.Memcached.DefaultNodeLocator">
		  <level value="Debug"/>
		</logger>
		<logger name="Enyim.Caching.Memcached.PooledSocket">
		  <level value="Info"/>
		</logger>
		<logger name="Enyim.Caching.Memcached.Protocol">
		  <level value="Info"/>
		</logger>
		<logger name="Membase.VBucketAwareOperationFactory">
		  <level value="Info"/>
		</logger>
		<logger name="Enyim.Caching.Memcached.MemcachedNode">
		  <level value="Info"/>
		</logger>
		<appender name="RollingLogFileAppender" type="log4net.Appender.RollingFileAppender">
		  <param name="File" value="LogFiles/"/>
		  <param name="AppendToFile" value="true"/>
		  <param name="MaxSizeRollBackups" value="10"/>
		  <param name="StaticLogFileName" value="false"/>
		  <param name="DatePattern" value="yyyy-MM-dd"/>
		  <param name="RollingStyle" value="Date"/>
		  <layout type="log4net.Layout.PatternLayout">
			<param name="ConversionPattern" value="%d{yyyy-MM-dd HH:mm:ss}[%thread] %-5level %c %L %F - %message%newline"/>
		  </layout>
		  <footer value="by anyx"/>
		</appender>
	  </log4net>

	  <enyim.com>
		<memcached>
		  <servers>
			<!--在这里添加你的缓存服务器地址，可以是多个，IP地址以及对应的端口-->
			<add address="172.19.50.185" port="11211" />
			<!--<add address="127.0.0.1" port="11211" />-->
		  </servers>
		  <!--这里进行连接池大小、连接超时设置等参数的配置-->
		  <socketPool minPoolSize="10" maxPoolSize="100" connectionTimeout="00:00:10" deadTimeout="00:02:00" />
		</memcached>
	  </enyim.com>  
	</configuration>