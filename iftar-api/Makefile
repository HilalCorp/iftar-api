# ------------------------------------------------------
# WARNING for PowerShell Users:
# Users of PowerShell might encounter issues using 'make' directly in PowerShell.
# If you're using PowerShell, you may need to run the commands manually or switch
# to another terminal (like Git Bash or WSL) to use 'make' effectively.
# 
# Alternatively, you can copy and paste the individual commands from the Makefile
# directly into the PowerShell terminal. For example:
# - To run `make run`, you would type `docker-compose up -d --build`
# ------------------------------------------------------

# Makefile for managing Docker containers and database migrations for the project

# Docker Compose command
DOCKER_COMPOSE = docker-compose -f docker-compose.yaml

# API container name (adjust this as needed)
API_CONTAINER = iftar-api

# Database container name (adjust this as needed)
DB_CONTAINER = iftar-db

# Commands
.PHONY: run restart-all stop rebuild-api migrate remove-migration update-db

# ------------------------------------------------------
# Command: run
# Description: Start the Docker containers (API + DB) and rebuild the images if necessary.
# Example: make run
# This command will build and start the containers in detached mode.
# ------------------------------------------------------
run:
	@echo "Starting Docker containers..."
	cd .. && $(DOCKER_COMPOSE) up -d --build

# ------------------------------------------------------
# Command: restart-all
# Description: Restart all running Docker containers without rebuilding the images.
# Example: make restart-all
# This command is useful when the code or configuration hasn't changed, 
# and you want to restart the containers.
# ------------------------------------------------------
restart-all:
	@echo "Restarting Docker containers..."
	cd .. && $(DOCKER_COMPOSE) up -d

# ------------------------------------------------------
# Command: stop
# Description: Stop all running Docker containers (API + DB).
# Example: make stop
# This command stops the containers, but does not remove them from the system.
# ------------------------------------------------------
stop:
	@echo "Stopping Docker containers..."
	cd .. && $(DOCKER_COMPOSE) down

# ------------------------------------------------------
# Command: rebuild-api
# Description: Rebuild and restart only the API container, leaving the DB container untouched.
# Example: make rebuild-api
# This is useful when you've changed something in the API code or Docker configuration related to the API.
# ------------------------------------------------------
rebuild-api:
	@echo "Rebuilding and restarting the API container..."
	cd .. && $(DOCKER_COMPOSE) up -d --build $(API_CONTAINER)

# ------------------------------------------------------
# Command: migrate
# Description: Create a new migration with a specified name.
# The command will prompt for the migration name.
# Example: make migrate
# This will ask you to enter the migration name interactively.
# It runs: dotnet ef migrations add <migration-name>
# ------------------------------------------------------
migrate:
	@echo "Creating a new migration..."
	@read -p "Enter migration name: " migration_name; \
	dotnet ef migrations add $$migration_name

# ------------------------------------------------------
# Command: remove-migration
# Description: Remove the last migration.
# Example: make remove-migration
# This command is useful if you made a migration but haven't updated the database yet.
# It runs: dotnet ef migrations remove
# ------------------------------------------------------
remove-migration:
	@echo "Removing the last migration..."
	dotnet ef migrations remove

# ------------------------------------------------------
# Command: update-db
# Description: Update the database to the latest migration.
# Example: make update-db
# This command will apply the latest migration to your database.
# It runs: dotnet ef database update
# ------------------------------------------------------
update-db:
	@echo "Updating the database..."
	dotnet ef database update
