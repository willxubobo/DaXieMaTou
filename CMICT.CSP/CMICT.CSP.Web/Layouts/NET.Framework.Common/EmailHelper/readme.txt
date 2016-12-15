1. 需要在Web.Config中增加如下配置项：
	<appSettings>
		<!-- smtp服务器地址和发件人帐号 10.0.15.22-->
		<add key="IfSendEmail" value="true"/>
		<add key="UseThirdPartMailServer" value="false"/>
		<add key="SMTPServerAddress" value="smtp.gmail.com"/>
		<add key="SMTPServerPort" value="587"/>
		<add key="SMTPUserAccount" value="xxxx@gmail.com"/>
		<add key="SMTPUserPassword" value="password"/>
    
		<add key="CompanyMailServerAddress" value="10.0.15.22"/>
		<add key="CompanyMailServerPort" value="25"/>
	</appSettings>

2. 用法：
	MailConfig mailCfg = new MailConfig();
    mailCfg.Body = mailBody;
    mailCfg.FromUser = "xxxx@xxx.com";
    mailCfg.ToUsers = toUserList;
    mailCfg.CcUsers = toUserList;    
    mailCfg.Subject = "mail subject";

	//使用公司内部的Exchange服务器来发送邮件	
    if (!useThirdPartMailServer)
    {
        mailCfg.Host = companyMailServerAddress;
        SmtpMailUtil.Sendmail(mailCfg);
    }
    else //使用非公司的外部第三方eMail服务器来发送邮件（譬如：腾讯邮箱、Gmail等）
    {
        mailCfg.Host = smtpServerAddress;
        mailCfg.Port = companyMailServerPort;
        mailCfg.Username = smtpUserAccount;
        mailCfg.Userpwd = smtpUserPassword;

        SmtpMailUtil.SendMailWithSLL(mailCfg);
    }