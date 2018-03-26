# javnarasprava.ba
Source code for community site javnarasprava.ba

# Javnarasprava.ba – technical documentation

Version 1.0

March 21, 2018
Prepared By: Enes Pelko @enespelko

### Version History

| Date | **Ver.** | **Authors** | Comments |
| ---- | -------- | ----------- | -------- |
| 04/15/2018 | 1.0 | Enes Pelko | Initial version |


### Table of Contents

1. Purpose of this Document
2. Executive Summary
3. Architecture Overview
4. Technolgoies used
5. Deployment Scenarios and prerequisites
    1. On dedicated server
    2. On Cloud
    3. Shared prerquisites
6. Functionality overview
    1. Admin page 
    2. Javnarasprava
    3. Pitaj Parlamentarce
    4. Peticije 
7. Instrumentation
8. Security
9. Configuration 
10 Localization
11 Data model
    1. List of tables with explanation
12. Azure Deployment for konsultimipublik.al  
    1. Subscription Data 
    2. Configured Azure Resources 
        1. RG: apps-kp-prod
        2. Infra-kp-prod



## 1 Purpose of this Document

The purpose of this document is to provide overview of javnarasprava.ba platform (Platform) implementation for maintenance and future development.

## 2 Executive Summary

Javnarasprava.ba is n-tier server-side web application build using ASP.NET MVC 5 Framework and other complementary technologies.

Supports on-premise or Azure cloud deployment scenarios.

Functionality is divided in 4 main modules: Amin for site administration,  &quot;Javna Rasprava&quot; for Law based discussions,, &quot;Pitaj Parlemantarca&quot; (Eng. Ask MPs) for engaging MPs with direct questions and Petitions to promote various community initiatives.

Instrumentation is externalized using Microsoft Application Insights and Google Analytics.

Authentication is done oAuth 2.0 protocol, with Facebook external login support. Authorization is role based.

Localization is supported using standard .resx where all user facing strings are translated in supported languages. There is custom implementation for email templates and URL localization.

Application Configuration is done using standard web.config file.

Data is stored in set of tables in SQL database.

For konsultimipublik.al application is deployed to Microsoft Azure Subscription.

## 3 Architecture Overview

Javnarasprava.ba is n-tier server-side web application. It uses SQL Database as backend database store.

For content storage it uses virtual directory in IIS or Azure Storage depending on deployment scenario.

Instrumentation is externalized using Microsoft Application Insights and Google Analytics.

Authentication is done oAuth 2.0 protocol, with Facebook external login support. Authorization is role based.

## 4 Technologies used

Javnarasprava.ba is build using Microsoft ASP.NET MVC 5 Framework and other complementary technologies.

Technologies used per application layer:

- Database – MS SQL Server and MS SQL Azure depending on deployment
- ORM – MS Entity Framework 6
- Web framework – MS ASP.NET MVC 5
- Client Side – jQuery, Bootstrap, Twitter API, Facebook Graph API, Tiny MCE.
- Instrumentation – MS Application Insights
- Security – Facebook federated login, MS Identity

## 5 Deployment Scenarios and prerequisites

Application is built to support different deployment options including on dedicated server or cloud.

### 5.1 On dedicated server

Prerequisites for hosting application on dedicated server are:

- Windows operating system
- MS .net framework 4.6 or above
- MS SQL Server 2016 or later, all editions

### 5.2 On Cloud

Application is tested and deployed on MS Azure cloud both in IaaS and PaaS deployment scenarios.

Prerequisites for hosting application on Azure

- SQL Azure database instance

### 5.3 Shared prerquisites

For all deployment scenarios prerequisites are:

- Instance of Application Instance for storing and analyzing telemetry data
- SMTP account for sending emails
- Scheduler component to initiate weekly email notifications

## 6 Functionality overview

From functional perspective Application is divided into modules each handling specific set of functionalities but with direct communication with other modules.

### 6.1 Admin page

Admin page has restricted user access which allows managing all the content on site.

Access is available only through direct link {Domain}/Admin and for users that have admin role assigned.

### 6.2 Javnarasprava

Main application module. In Albanian: &quot;Konsultimi public&quot;. It represents a law catalog that enables detailed presentation of laws, voting on laws, pointed out topics, comments of selected experts and general users and posting questions to MPs related to laws.

### 6.3 Pitaj Parlamentarce

In English &quot;Ask MPs&quot; and in Albanian &quot;Pyesni deputetët&quot;. Module that enables direct communication with members of parliament by posting questions related or unrelated to current laws. Also, users can give likes or dislikes to questions and answers and share their opinion on them.

### 6.4 Peticije

In English &quot;Petitions&quot; and in Albanian &quot;Peticionet&quot;. In this module users can start petitions on any topics. Registered users can sign petitions and give their support by doing that. Portal admins can define milestones, which define actions that will be undertaken for every petition that reaches that milestone.

## 7 Instrumentation

All telemetry data is gathered and stored in MS Application Insights service from MS Azure.

Application Insights instance is configured in applications web.config file.

How to analyze data in AI please refer to MS official documentation at: [https://docs.microsoft.com/en-us/azure/application-insights/](https://docs.microsoft.com/en-us/azure/application-insights/)

## 8 Security

Certain parts of the application are allowed only for registered users. Application uses oAuth 2.0 protocol for authentication and role base authorization to restrict user access.

Users can login using local account created using verified email address or using Facebook account using federated login.

Overall good development practices are applied to avoid common security exploits like SQL injection, XSS, etc.

## 9 Configuration

Configuration is done using web.config file. Following configuration values are available:

Connection strings:

- **DefaultConnection** : Connection string do database server. Standard SQL server connection string

AppSettings:

- RepresentativeImage.RootPath: Root network path to Representative Images. Depending on store can be relative path in IIS or full path to CDN.
- LawImage.RootPath: Root network path to Law Images. Depending on store can be relative path in IIS or full path to CDN.
- LawSectionImage.RootPath: Root network path to Law Section Images. Depending on store can be relative path in IIS or full path to CDN.
- LawText.RootPath: Root network path to Law Texts. Depending on store can be relative pathin IIS or full path to CDN.
- PetitionImage.RootPath: Root network path to Petition Images. Depending on store can be relative path in IIS or full path to CDN.
- QuizImage.RootPath: Root network path to Quiz Images. Depending on store can be relative path in IIS or full path to CDN.
- NewsImage.RootPath: Root network path to News Images. Depending on store can be relative path in IIS or full path to CDN.
- PetitionProgressImage.RootPath: Root network path to Petition Progress Images. Depending on store can be relative path in IIS or full path to CDN.
- StorageConnectionString: Connection string for Azure Storage if used to store images.
- CDN.RootPath : Content path format if local IIS folder is used. This is used for storing files. Example value: &quot;~/Content/CDN/{0}{1}&quot;
- CDN.BlobContainer: Name of blob container to store content to if Azure Storage Account is used for storing content
- EMAIL.SMTP: SMTP server used for sending emails
- EMAIL.PORT: SMTP server port used for sending emails
- EMAIL.USERNAME: SMTP Server username
- EMAIL.PASSWORD: SMTP Server password
- EMAIL.FROM: From email filed for sent emails
- EMAIL.TOOVERRIDE: For testing purposes to field in all emails can be overridden
- EMAIL.TOADD: Send all emails as bcc to addresses in this list
- JavnaRasprava.Index.PointedOutSections: Number of pointed out sections on homepage
- JavnaRasprava.Index.Laws: Number of pointed out lows on homepage
- JavnaRasprava.Index.TopRepresentatives: Number of MPs on homepage
- BaseAddress: FQDN of site, used for redirects and email links
- BaseTenantAddress: FQDN of site with placeholder for tenant {0}, used for tenant redirects
- CookieDomain: Domain name for the cookie
- CookieName: Name of authentication cookie
- FB.AppId: FB App Id
- FB.Secret: FB App Secret
- GA.Include: True if Google analytics should be included
- GA.Code: Google analytics code
- RunSeed: True if to insert test date on start
- AI.Key: Application insights telemetry id
- IntegrationToken: This value must be provided by external scheduler for sending emails.
- ValidCultures: List of valid cultures. First one is default. Default values are &quot;sq,bs&quot;
- FeatureToggle.Instance: Current instance ALB or BS. Used to turn off and on features specific for instance.
- FeatureToggle.DefaultPCode: Default parliament code. Al for Albania, or bah for BS
- Facebook.LawUrlTemplate: Template for Facebook comment URL. Must be unique as we can get to page from multiple links sample value &quot;http://www.jr.ba/JavnaRasprava/LawDetails?lawId={0}&quot;

## 10 Localization

Three components are used to support localization

- Resx files

For request based content standard .Net resx files are used. They are located at /App\_GlobalResources folder in source control. There are total 2 files used GlobalLocalization.resx and StaticPageData.resx. These are key value files and for each new culture new file with translations must be created

- For managing default URL routes per culture /App\_GlobelResource/Routes.resx is used to determine which route name is default for which culture.
- For email templates translations from  /App\_GlobelResource/EmailTemplates are used. Only one culture per instance can be used.

## 11 Data model

Data store used for application is standard SQL server relational database. On application layer Entity Framework 6 is used for ORM.

### 11.1 List of tables with explanation

- AnswerLikes:

Holds user likes for Answers.

- Answers:

Holds Answers on questions when given by Representatives.

- AnswerTokens:

AnwerToken object is created to ensure that only one Answer can be created per Representative and Question. So Guid is generated which is sent to Representative in email as part of answer link.

- AspNetRoles:

Holds user role in application.

- AspNetUserLogins:

Holds information about external logins for users. Currently only Facebook is used.

- AspNetUserRoles:

User to role association.

- AspNetUsers:

Holds user data.

- ExpertComments:

Expert comments for a Law.

- Experts:

Experts are partners of application that can provide comments on laws. Experts and Expert comments are managed by Admins on Admin page. Comments are showed on law page in dedicated area.

- ExternalLinks:

External links for representatives which can provide more information on them.

- InfoBoxItems:

Holds references to objects that will be displayed on home page info boxes. Partition is parliament, and type is actual info box.

- LawCategories:

List of categories for laws.

- LawCustomVotes:

Law and Law Section vote is structured user opinion for a Law or Law Section. Besides two predefined vote options Yes and No, admins can define additional vote options that have more details. Users can submit their own votes, but it must go through approval process by admins.

- LawRepresentativeAssociations:

Admins can select a list of representatives that are directly related to a Law. These Representatives will be displayed as default options for posting questions related to that Law.

- Laws:

Law data.

- LawSectionCustomVotes:

Law and Law Section vote is structured user opinion for a Law or Law Section. Besides two predefined vote options Yes and No, admins can define additional vote options that have more details. Users can submit their own votes, but it must go through approval process by admins.

- LawSections:

Law sections are topics in Law that are handled as separate topic within the law. Users can vote on Law sections

- LawSectionVotes:

User votes on Law Section.

- LawVotes:

User votes on Law.

- Locations:

List of locations that users can select on sign up page. This is later used in filters for votes.

- News:

Holds news data.

- ParliamentHouses:

Holds data of Parliament houses. If parliament house has only one, it also must be defined here.

- Parliaments:

Application supports multiple parliaments. This holds information for parliaments.

- Parties:

List of parties that can be associated to Representatives and Users on sign up page.

- PetitionProgresses:

Petition Progress is milestone in petition votes. Site admins use this to define actions that will be undertaken for a petition if it reaches defined number of votes. Petition progress can be related to a representative, in which case number of votes is the actual number of votes won by representative, or it can be custom milestone.

- Petitions:

Holds petition data.

- PetitionSignatures:

Petition signatures by a user.  Only registered users can sign petitions and only once.

- Procedures:

Types of Law procedures.

- QuestionLikes:

User likes for a question.

- QuestionMessages:

Representatives are notified by email for each question they receive. For every email sent a record is created to track number of notifications sent to Representatives.

- Questions:

Questions made by user or admins for a law or directly to representative.

- QuizItems:

Quizzes are fast way to give votes on Laws and Law Sections. Quiz Item is reference to a Law or Law Section for a quiz.

- Quizs:

Quizzes are fast way to give votes on Laws and Law Sections.

- Regions:

Locations are organized in reasons. Used for better filtering.

- RepresentativeAssignments:

Representatives can have different assignments within parliament, such as commissions or groups.

- Representatives:

Holds Representative Data

- UserRepresentativeQuestions:

Relationship between User, Representative and Question.

## 12 Azure Deployment for konsultimipublik.al

For Konsultimipublik.al instance javnarasprava.ba application is deployed to Microsoft Azure subscription.

### 12.1 Subscription Data

Subscription Name: Microsoft Azure Sponsorship

### 12.2 Configured Azure Resources

Two Resource Groups are used to separate infrastructure resources and application resources

#### 12.2.1 RG: apps-kp-prod

Holds application resources that are version specific:

| **Name** | **Type** | **Description** |  
| --- | --- | --- | 
| **konsultimipublik** | App Service | Application service for production instance |  
| **konsultimipublikPlan** | App Service plan | Service plan (web servers) for production instance |  

#### 12.2.2 Infra-kp-prod

Holds infrastructure resources that will not change with application version

| **Name** | **Type** | **Description** |   |
| --- | --- | --- | --- |
| **konsultimipublik** | Storage Account | Holds file content | 
| **konsultimipublik.al** | DNS zone | Domain controller for konsultimipublik.al domain |  
| **weeklyreports** | Scheduler Job Collection | Scheduler for sending weekly reports |  
| **konsultimipublik** | Application Insights | Application telemetry data |  
| **konsultimipublikdbserver** | SQL server | SQL Server resource |  
| **konsultimipublik.data** | SQL Database | Production SQL database |  
