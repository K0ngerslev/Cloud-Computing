# SupportWebApp

En Blazor Web App hvor man kan oprette supporthenvendelser og se dem i en liste. Henvendelserne bliver gemt i Azure Cosmos DB.

Projektet er lavet til M4.04 – CosmosDB med WebApp.

## Hvad kan den?

- Oprette en supporthenvendelse med navn, telefon, mail, kategori og beskrivelse.
- Se alle supporthenvendelser i en liste.
- Gemme data i Cosmos DB.
- Bruge validering på felterne i formularen.

## Cosmos DB

Vi bruger:

- Resource group: IBasTestGroup
- Location: swedencentral
- Database: IBasSupportDB
- Container: ibassupport
- Partition key: /category

Hvis databasen skal oprettes igen, kan man bruge:

    az provider register --namespace Microsoft.DocumentDB --wait

    az group create \
      --name IBasTestGroup \
      --location swedencentral

    az cosmosdb create \
      --name <UNIKT-NAVN> \
      --resource-group IBasTestGroup \
      --enable-free-tier true

    az cosmosdb sql database create \
      --account-name <UNIKT-NAVN> \
      --resource-group IBasTestGroup \
      --name IBasSupportDB

    az cosmosdb sql container create \
      --account-name <UNIKT-NAVN> \
      --resource-group IBasTestGroup \
      --database-name IBasSupportDB \
      --name ibassupport \
      --partition-key-path "/category"

Connection string kan hentes med:

    az cosmosdb keys list \
      --name <UNIKT-NAVN> \
      --resource-group IBasTestGroup \
      --type connection-strings \
      --query "connectionStrings[0].connectionString" \
      --output tsv

Connection string skal ikke lægges direkte på GitHub.

## Kør projektet

Connection string sættes som user-secret:

    dotnet user-secrets set "CosmosDb:ConnectionString" "<connection string>"

Derefter startes projektet:

    dotnet run

## Status

Det har vi lavet indtil videre:

- Blazor Web App er oprettet.
- Cosmos DB er oprettet i Azure.
- Database og container er oprettet.
- Der er lavet en model til supporthenvendelser.
- Der er lavet validering af input.
- Man kan oprette supporthenvendelser.
- Man kan se supporthenvendelser i en liste.
- Cosmos DB bruges til at gemme data.
- Connection string bliver gemt med user-secrets.

Det mangler:

- Der er ikke login eller brugerrettigheder endnu.
- Man kan ikke redigere, lukke eller slette supporthenvendelser.
- Projektet er ikke deployet til Azure endnu.

## Næste skridt

Det næste vil være at få projektet deployet til Azure og få Cosmos DB-forbindelsen til at virke der.

Derefter kunne man lave status på supporthenvendelser, fx åben/lukket, og mulighed for at ændre en henvendelse.