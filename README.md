# CodeComp-DotNet-Player

## Prerequisites to play
- Git
- Editor of your choice(eclipse/intelliJ recommended)

## How to play from your local machine with 'dumb' algo
We have written all boilerplate code to get you started within 5 minutes. You kust need to make 'MySmartAlgo' smarter. You can improve your algo by playing with test server.
- Clone this repo using : git clone https://github.com/git-dev-eng/codecomp-player-dotnet
- cd codecomp-player-dotnet
- change 'team' and 'password' in appsettings.json inside codecomp.player
- Using editor of your choice, Run as Dotnet Console Application: dotnet run and start playing.
- see url for live score : https://mmd-2021-ui-g6titslaoq-ew.a.run.app/Live

## Final Competition Day
- You need to deploy your algo in Google Cloud Platform to connect to game server.
- You can fork this repository, modify algo, check-in and deploy into Google Cloud from your github repository.
-  Step-by-step guide to deploy a Dotnet application on google cloud is given below:

## Step-by-step guide

## Setting up a GCP Account

**You can skip this step if you already have a GCP Project

* Go to [Google Cloud](https://cloud.google.com/) and click on **Get Started for FREE**.
* Login using your gmail account, choose your country, accept terms and conditions and click **Continue**.
* In the next step, fill your details, like account type, Name, Address, credit card details, tax information, etc. If you have old Gmail account and all the information is already there it would take it and you might not have to fill all the details.
* After filling all the details click on **Start my free trial**.
* Google will setup your cloud account and in few seconds your Google Cloud Platform account will be ready to start deploying applications on it. It will look like below:

![Project Setup](/img/gcp-project-setups-modified.jpg)

## Creating new GCP Project

* Go to [Manage Resources Page](https://console.cloud.google.com/cloud-resource-manager) and click on **New Project**
* Fill in the Project Name and keep location as it is. You can also **Edit** Project ID according to availability. Once done click on **Create** and your New GCP Project will be created.

![New Project](/img/new-project-description-fied.jpg)

* After you have created your project. Go to Billing and click on **Link Billing Account**

![Link Billing](/img/billing-account-fied.jpg)

* Select Billing Account from dropdown menu and click on **Set Account**

![Set Billing Account](/img/set-billing-account-modified.jpg)

## Creating a Service Account(SA) and SA Key in GCP Project

* Go to **Navigation Menu(Top left Corner) > IAM & Admin > Service Accounts**
  * Click on **Create Service Account**
  * Under **Service Account Details** provide service account **name** and **description** of your choice and click on **Create**

![Service Account Details](/img/service-account-details-modified.jpg)
  
* Under **Service Account Permissions** , add the owner role as shown below and click on **Continue**:

  ![Service Account Permissions](/img/service-account-permissions-modified.jpg)
  
  * Keep **User Access Section** unchanged and click on **Done**
  * On Service Account Page click on Actions hamburger menu of Service Account you just created and Click on  Manage Keys
  
![Service Account Manage Key](/img/manage-key.JPG)

  * Click on **Add Key** and then **Create new key**
  
![Service Account Create Key](/img/create-new-key.JPG)  

* Select **JSON** option and Click on **Create**. A JSON file will get downloaded on your local system, save it we will need it later*

![Service Account JSON](/img/create-json-key.JPG)

## Creating Github Secrets

* Fork this repository
* Go to **Settings > Secrets** in your forked repository.
* Click on **New Repository Secret** and add Name as **PROJECT_ID** and value as Project ID of your GCP Project and click on Add Secret

![New Secret](/img/secret-project.JPG)

* Again click on New Secret and add Name as **GOOGLE_APPLICATION_CREDENTIALS** and value as contents of json file that you downloaded and click on Add Secret
  
* KEEP IN MIND, not to change Secret Names otherwise you will need to alter workflow in .github/workflows directory

## Deploying the bot

* The workflow to deploy the application is already set in .github/workflows/cloud.yaml file in your forked repo. The workflow can be triggered manually by visiting actions tab in your forked github repo or automatically whenever some code is pushed.
* Firstly, Visit the Actions tab in your forked repo and Click **I understand my workflows, go ahead & enable them**

![Enable-workflows](/img/understand-workflows.JPG)

* Open the workflow by clickng on workflow name - **Build and Deploy to GAE** in this case

![Open-workflow](/img/open-workflow.JPG)

* Trigger the workflow manually by visiting actions tab in your github repo and let Github Actions do the magic.

![Run-Workflow](/img/run-workflow.JPG)
  
* In the Actions tab in your forked repo and open the latest the workflow run and see the progress. You will see something like this when pipeline completes.

![Pipeline-Run](/img/pipeline-run.JPG)
  
* Once workflow is successful, head towards google cloud console and search app engine on search bar, you can see the bot deployed there as default service.

![App Engine Services](/img/app-engine-services.JPG)
  
## Checking Logs
  - Go to App Engine > Services and click on Tools in your deployed service and select Logs which will open the logs of application.
  
  - In case if you see the new logs explorer switch to legacy logs viewer by going to OPTIONS> Go Back to the Legacy Logs Viewer
![New Logs Explorer](/img/switch-logs-explorer.JPG)  
  - You can click on Play button on top to start streaming live logs.

![Logs](/img/logs.JPG)  
  
- It might take some time to service to get up and running or logs to load initially

## Happy Coding :smile:  



