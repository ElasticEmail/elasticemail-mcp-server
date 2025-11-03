## Requirments
.NET SDK 10 or higher
To work properly, it needs connection to Elastic Email API.
Open port 5001 for http.

## Copilot integration
Use VS Code
Open Chat window.
Choose 'Agent' mode
Use settings.json file to connect to mcp server (open folder containing this file).
You should see little controls near "servers".
Add a settings file as a context to agent.
Open the "Select tools..." window (click on screwdriver and wrench icon)
Find mcp server and make sure all tools selected.
Click "Start"

### example configuration for agent:
```
"my-server-unique-number": {
    "url": "http://localhost:5001/",
    "headers": {
        "X-Auth-Token": <api_key>
    }
}
```

## Tools

- HealthCheck.IsReady - tool to check Elastic Email api server is available and responding
- CampaignManagement.CreateCampaign - creates campaign in the system
- CampaignManagement.ListCampaigns - lists existing campaigns
- CampaignManagement.GetCampaign - get campaign details
- CampaignManagement.Pause -  stops campaign processing
- CampaignManagement.UpdateCampaign - change campaign data (can be started again by a status edition)
- CampaignManagement.GetAllCampaignStatistics - get all campaigns numbers like number of opened emails
- CampaignManagement.GetCampaignStatistics - get campaign (by name) numbers like opened, clicked, etc
- SegmentsManagement.CreateSegment - create new segment
- SegmentsManagement.GetSegments - get list of existing segments
- SegmentsManagement.GetSegment - get segment by name
- TemplateManagement.FetchTemplate - get template details
- TemplateManagement.FetchTemplates - get list of templates, filtering by type and accessibility
- EmailSending.SendBulkEmails - send email to multiple users (can use contacts list), provide content or use template
- EmailSending.SendTransactionalEmail - send email to contacts, can provide template
- ContactsManagement.FetchContacts - get contacts list
- ContactsManagement.FetchContactHistory - gets contact history (if enabled)
- ContactsManagement.FetchLists - get all contact lists
- ContactsManagement.FetchList - get contact list details
- ContactsManagement.FetchListContacts - get contacts on list (by name)
- ContactsManagement.AddContactToList - add contact to list
- ContactsManagement.RemoveContactsToList - remove provided contact from list (by name)
- ContactsManagement.CreateList - create new contacts list
- ContactsManagement.AddContact - add new contact
- ContactsManagement.DeleteContacts - delete contacts (provide emails)
- ContactsManagement.UploadContacts - upload and add new contacts from file

