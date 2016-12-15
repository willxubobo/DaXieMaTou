1. 用法
	private ILogger log = LoggerManager.GetLogger("Platform");
	log.Info("xxxxxxx");

2. Web.Config中关于Log4net的配置如下：
 
<configuration>
  <configSections>
    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />
  </configSections>


  <!-- This section contains the log4net configuration settings -->
  <log4net>
    <!-- Define some output appenders -->

    <appender name="PlatformLogFileAppender" type="log4net.Appender.RollingFileAppender">
      <param name="File" value="log\PlatformLog\Log.txt" />
      <param name="AppendToFile" value="true" />
      <param name="RollingStyle" value="Composite" />
      <param name="DatePattern" value="yyyy-MM-dd" />
      <param name="MaxSizeRollBackups" value="10" />
      <param name="CountDirection" value="1" />
      <param name="MaximumFileSize" value="10MB" />
      <param name="StaticLogFileName" value="false" />
      <layout type="log4net.Layout.PatternLayout">
        <param name="ConversionPattern" value="%m%n" />
      </layout>
    </appender>

    <appender name="RollingLogFileAppender" type="log4net.Appender.RollingFileAppender">
      <param name="File" value="log\log.txt" />
      <param name="AppendToFile" value="true" />
      <param name="MaxSizeRollBackups" value="2" />
      <param name="MaximumFileSize" value="100KB" />
      <param name="RollingStyle" value="Size" />
      <param name="StaticLogFileName" value="true" />
      <layout type="log4net.Layout.PatternLayout">
        <param name="Header" value="[Header]\r\n" />
        <param name="Footer" value="[Footer]\r\n" />
        <param name="ConversionPattern" value="%d [%t] %-5p %c [%x] - %m%n" />
      </layout>
    </appender>
    <appender name="ConsoleAppender" type="log4net.Appender.ConsoleAppender">
      <layout type="log4net.Layout.PatternLayout">
        <param name="ConversionPattern" value="%d [%t] %-5p %c [%x] &lt;%X{auth}&gt; - %m%n" />
      </layout>
      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" />
    </appender>

    <!-- Set root logger level to ERROR and its appenders -->
    <root>
      <level value="DEBUG" />
      <appender-ref ref="RollingLogFileAppender" />
      <appender-ref ref="ConsoleAppender" />
    </root>

    <!-- Print only messages of level DEBUG or above in the packages -->
    <logger name="IBatisNet.DataMapper.Configuration.Cache.CacheModel">
      <level value="DEBUG" />
    </logger>
    <logger name="IBatisNet.DataMapper.Configuration.Statements.PreparedStatementFactory">
      <level value="DEBUG" />
    </logger>
    <logger name="IBatisNet.DataMapper.LazyLoadList">
      <level value="DEBUG" />
    </logger>
    <logger name="IBatisNet.DataAccess.DaoSession">
      <level value="DEBUG" />
    </logger>
    <logger name="IBatisNet.DataMapper.SqlMapSession">
      <level value="DEBUG" />
    </logger>
    <logger name="IBatisNet.Common.Transaction.TransactionScope">
      <level value="DEBUG" />
    </logger>
    <logger name="IBatisNet.DataAccess.Configuration.DaoProxy">
      <level value="DEBUG" />
    </logger>
    <logger name="Platform">
      <level value="All" />
      <appender-ref ref="PlatformLogFileAppender" />
    </logger>
  </log4net>
</configuration>
