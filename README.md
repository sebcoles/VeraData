# VeraData

VeraData is a component for downloading Veracode data for an Application Profile via the XML APIs and storing the contents within MS SQL Server with a sensible schema. The model is then ready for you to connect your favourite BI tools such as Qlik, Power BI etc.

The Models includes all scans with their flaws, mitigations and callstacks. For now it is all or nothing for a given appid.

## Usage

You must retrieve your App ID either through the Platform or through the current API wrappers. Update the appsetiings.json to include your database connection string and then run.

```
dump --appid "__YOUR_APP_ID__"

```
By default you will provided with the total scans available and confirmation to download.

Use the ```-f``` flag to force this to skip the confirmation and download the data anyway.


