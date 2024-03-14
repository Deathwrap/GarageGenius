create table if not exists service_categories
(
    id   serial
        primary key,
    name varchar not null
);

alter table service_categories
    owner to garage_genius_admin;

create table if not exists services
(
    id                  serial
        primary key,
    category_id         integer not null,
    name                varchar not null,
    execution_time      numeric,
    standard_hour_price numeric
);

alter table services
    owner to garage_genius_admin;

create table if not exists clients_confirmed
(
    id        uuid not null
        primary key,
    name      varchar,
    email     varchar,
    pass_hash varchar
);

alter table clients_confirmed
    owner to garage_genius_admin;

create table if not exists clients_to_confrim
(
    id            uuid                     not null
        primary key,
    name          varchar,
    email         varchar,
    pass_hash     varchar,
    creation_date timestamp with time zone not null,
    code          varchar
);

alter table clients_to_confrim
    owner to garage_genius_admin;

create table if not exists refresh_tokens
(
    id             serial
        primary key,
    token_owner_id uuid not null,
    session_id     uuid,
    token          varchar,
    creation_date  timestamp with time zone
);

alter table refresh_tokens
    owner to garage_genius_admin;

create table if not exists cars
(
    id                  serial
        primary key,
    client_id           uuid not null,
    brand               varchar,
    model               varchar,
    year                integer,
    registration_number varchar,
    vin                 varchar
);

alter table cars
    owner to garage_genius_admin;

create table if not exists positions
(
    id   serial
        primary key,
    name varchar not null
);

alter table positions
    owner to garage_genius_admin;

create table if not exists workers
(
    id          uuid    not null
        primary key,
    position_id integer not null,
    name        varchar not null,
    login       varchar not null,
    pass_hash   varchar not null
);

alter table workers
    owner to garage_genius_admin;

create table if not exists clients_offline
(
    id           uuid    not null
        primary key,
    name         varchar not null,
    email        varchar,
    phone_number varchar not null
);

alter table clients_offline
    owner to postgres;


