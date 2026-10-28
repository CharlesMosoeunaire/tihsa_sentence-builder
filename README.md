# Sentence Builder

Full-stack web app for building sentences from words grouped by word type
(Noun, Verb, Adjective, Adverb, Pronoun, Preposition, Conjunction, Determiner, Exclamation).

| Layer | Technology |
|---|---|
| Frontend | Angular 20 (standalone components, signals), installable PWA, responsive layout |
| Backend | ASP.NET Core Web API (.NET 10), Entity Framework Core |
| Database | SQL Server 2022 (only the API connects to it) |
| Containers | Dockerfiles for API and web, `docker-compose.yml` for the whole stack |

## Run with Docker

```powershell
docker compose up --build
```

- Web: http://localhost:4200
- API: http://localhost:5080/api/words (OpenAPI document at `/openapi/v1.json`)

The API applies the EF migration on startup, which creates the schema and seeds the word lists.

## Run locally

1. Start SQL Server: `docker run -d --name sentence-sql -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Sentence_Builder_123" -p 1433:1433 mcr.microsoft.com/mssql/server:2022-latest`
2. API: `cd api/SentenceBuilder.Api; dotnet run --urls http://localhost:5080`
3. Web: `cd web; npm install; npm start`, then open http://localhost:4200

## REST API

| Method | Route | Purpose |
|---|---|---|
| GET | `/api/words` | All word types, each with its selectable words |
| GET | `/api/sentences` | All submitted sentences (newest first) |
| GET | `/api/sentences/{id}` | One sentence |
| POST | `/api/sentences` | Submit a new sentence: `{ "wordIds": [1, 14, 22] }` |
| PUT | `/api/sentences/{id}` | Replace the words of an existing sentence (used by edit) |

Validation: a sentence with no words, or with unknown word ids, returns `400` with a problem-details body.

## Design decisions

- **No hardcoded values.** Word types and words are rows in the database, created by the EF migration seed. The frontend builds every list from API responses. The API URL is read at runtime from `config.json` (in containers it is generated from the `API_BASE_URL` environment variable). The API's connection string, CORS origins and migrate-on-startup flag come from configuration or environment variables.
- **Sentences reference words by id**, stored with a position, so order is preserved and unknown words are rejected.
- **PUT for editing** keeps one record per sentence instead of creating duplicates.
- **Database access** is only from the API; the browser never talks to SQL Server.

## Azure deployment (ready, not deployed)

1. **Azure SQL Database**: create a server and database `SentenceBuilder`.
2. **API**: Azure App Service (Linux, .NET 10) or Container Apps using `api/Dockerfile`. Set application settings:
   - `ConnectionStrings__DefaultConnection` = Azure SQL connection string
   - `Cors__AllowedOrigins__0` = the web app's URL
   - `Database__MigrateOnStartup` = `true`
3. **Web**: Azure Static Web Apps (build output `dist/sentence-builder-web/browser`) or Container Apps using `web/Dockerfile` with `API_BASE_URL` set to the API's URL. For Static Web Apps, edit `config.json` before deploying.
