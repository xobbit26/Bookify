#!/bin/bash

# Это скрипт миграции базы данных

# Извлечённые переменные для улучшения читаемости

MIGRATION_NAME=$1
INFRASTRUCTURE_PROJECT=Bookify.Infrastructure
STARTUP_PROJECT=Bookify.Api

dotnet ef migrations add <MigrationName> --project Bookify.Infrastructure --startup-project Bookify.Api

# Команда EF миграции базы данных

dotnet ef database update "$MIGRATION_NAME" --project "$INFRASTRUCTURE_PROJECT" --startup-project "$STARTUP_PROJECT"