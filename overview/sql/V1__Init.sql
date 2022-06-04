create table config
(
    key   varchar(25)
        constraint config_pk
            primary key,
    value text not null
);

create table organization
(
    id uuid
        constraint organization_pk
            primary key
);

create table application
(
    id uuid
        constraint application_pk
            primary key
);

