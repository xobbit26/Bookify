#!/bin/bash

# Скрипт миграции базы данных

MIGRATION_NAME=$1
INFRASTRUCTURE_PROJECT=Bookify.Infrastructure
STARTUP_PROJECT=Bookify.Api

dotnet ef migrations add "$MIGRATION_NAME" --project "$INFRASTRUCTURE_PROJECT" --startup-project "$STARTUP_PROJECT"

dotnet ef database update "$MIGRATION_NAME" --project "$INFRASTRUCTURE_PROJECT" --startup-project "$STARTUP_PROJECT"
