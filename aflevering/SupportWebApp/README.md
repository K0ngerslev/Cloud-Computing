# SupportWebApp

En .NET Blazor Web App, hvor brugere kan oprette **supporthenvendelser** og se en oversigt over alle henvendelser. Henvendelserne gemmes i en **Azure Cosmos DB** (NoSQL) database.

Projektet er lavet som afleveringsopgave **M4.04 – CosmosDB med WebApp** i faget Cloud Computing.

## Funktioner

- **Opret henvendelse** (`/create-support`): formular med navn, telefon, e-mail, kategori og beskrivelse. Felterne valideres med data annotations, før henvendelsen gemmes i Cosmos DB.
- **Se henvendelser** (`/support-list`): tabel med alle henvendelser fra Cosmos DB, nyeste først.

## Arkitektur

| Del | Fil | Beskrivelse |
|---|---|---|
| Model | `Model/SupportMessage.cs`, `Model/User.cs` | En henvendelse med indlejret brugerinfo. `id` og `category` (partition key) mappes til de feltnavne, Cosmos DB forventer. |
| Service | `Service/DataService.cs` | Håndterer forbindelsen til Cosmos DB via `CosmosClient` og har metoder til at indsætte og hente henvendelser. |
| DI | `Program.cs` | `DataService` registreres som singleton, så hele appen deler én `CosmosClient`. |
| Sider | `Components/Pages/CreateSupport.razor`, `Components/Pages/SupportList.razor` | Razor-sider til oprettelse og visning af henvendelser. |

NuGet-pakker: `Microsoft.Azure.Cosmos` og `Newtonsoft.Json`.

## Opret Cosmos DB-databasen med `az`

Kræver [Azure CLI](https://learn.microsoft.com/cli/azure/install-azure-cli). Skift værdierne i variablerne ud efter behov (eksemplet er skrevet til PowerShell).

```powershell
# Log ind i Azure
az login

# Navne
$RG        = "ibas-rg"
$LOCATION  = "northeurope"
$ACCOUNT   = "ibas-db-account-16038"   # skal være globalt unikt
$DATABASE  = "IBasSupportDB"
$CONTAINER = "ibassupport"

# 1. Resource group
az group create --name $RG --location $LOCATION

# 2. Cosmos DB-konto (NoSQL, serverless)
az cosmosdb create `
  --name $ACCOUNT `
  --resource-group $RG `
  --kind GlobalDocumentDB `
  --locations regionName=$LOCATION `
  --capabilities EnableServerless

# 3. Database
az cosmosdb sql database create `
  --account-name $ACCOUNT `
  --resource-group $RG `
  --name $DATABASE

# 4. Container med partition key /category
az cosmosdb sql container create `
  --account-name $ACCOUNT `
  --resource-group $RG `
  --database-name $DATABASE `
  --name $CONTAINER `
  --partition-key-path "/category"

# 5. Hent connection string
az cosmosdb keys list `
  --name $ACCOUNT `
  --resource-group $RG `
  --type connection-strings `
  --query "connectionStrings[0].connectionString" `
  --output tsv
```

## Kør projektet lokalt

1. Sæt database- og containernavn i `appsettings.json`:

   ```json
   "CosmosDb": {
     "ConnectionString": "",
     "DatabaseName": "IBasSupportDB",
     "ContainerName": "ibassupport"
   }
   ```

2. Gem connection string som **user-secret**, så den ikke kommer med i Git:

   ```
   dotnet user-secrets set "CosmosDb:ConnectionString" "<connection string fra trin 5>"
   ```

3. Start appen:

   ```
   dotnet run
   ```

   Åbn den URL, der vises i konsollen.

## Status

**Det har vi nået:**
- Blazor Web App oprettet ud fra `dotnet new blazor`-templaten.
- Model for supporthenvendelser med validering (påkrævede felter, gyldig e-mail og telefon, længde på beskrivelse).
- Serviceklasse til Cosmos DB, registreret med dependency injection.
- Side til oprettelse af henvendelser, som gemmes i Cosmos DB.
- Side med oversigt over alle henvendelser.
- Navigation mellem forside, oprettelse og oversigt. Counter- og Weather-siderne er fjernet.
- Connection string holdes ude af repository'et via user-secrets.

**Det mangler:**
- Man kan ikke redigere, lukke eller slette en henvendelse.
- Der er ingen login, så alle kan se alle henvendelser.
- Appen kører kun lokalt og er ikke deployet til Azure.

**Næste skridt:**
- Deploye appen til Azure App Service og gemme connection string som App Setting eller i Azure Key Vault.
- Bruge Managed Identity i stedet for en nøgle til at forbinde til Cosmos DB.
- Tilføje status på henvendelser (fx *åben*/*lukket*) og mulighed for at opdatere dem.
- Tilføje filtrering på kategori i oversigten (kategori er partition key, så forespørgsler pr. kategori er billige).
