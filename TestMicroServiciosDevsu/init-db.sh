#!/bin/bash
# Este script se ejecuta AUTOMÁTICAMENTE la primera vez que el contenedor de

set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" <<-EOSQL
    CREATE DATABASE person_db ;
    CREATE DATABASE account_db;
EOSQL
