create table config
(
    key   varchar(25) not null
        constraint config_pk
            primary key,
    value text        not null
);

alter table config
    owner to postgres;

create table organization
(
    id   uuid not null
        constraint organization_pk
            primary key,
    name text,
    acls jsonb
);

alter table organization
    owner to postgres;

create table application
(
    id   uuid not null
        constraint application_pk
            primary key,
    org  uuid
        constraint application_organization_id_fk
            references organization
            on delete set null,
    name text
);

alter table application
    owner to postgres;

create table pipeline
(
    id           uuid not null
        constraint pipeline_pk
            primary key,
    name         text,
    organization uuid
        constraint pipeline_organization_id_fk
            references organization
            on delete set null
);

alter table pipeline
    owner to postgres;

create table pipeline_version
(
    id        uuid not null
        constraint pipeline_version_pk
            primary key,
    name      text,
    pipeline  uuid
        constraint pipeline_version_pipeline_id_fk
            references pipeline
            on delete cascade,
    variables jsonb,
    code      text
);

alter table pipeline_version
    owner to postgres;

create unique index pipeline_version_name_uindex
    on pipeline_version (name);

