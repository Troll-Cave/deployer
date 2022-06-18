CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE config (
    key text NOT NULL,
    value text NOT NULL,
    CONSTRAINT "PK_config" PRIMARY KEY (key)
);

CREATE TABLE organization (
    id uuid NOT NULL,
    name text NOT NULL,
    CONSTRAINT "PK_organization" PRIMARY KEY (id)
);

CREATE TABLE pipeline (
    id uuid NOT NULL,
    name text NOT NULL,
    org uuid NULL,
    CONSTRAINT "PK_pipeline" PRIMARY KEY (id),
    CONSTRAINT "FK_pipeline_organization_org" FOREIGN KEY (org) REFERENCES organization (id) ON DELETE SET NULL
);

CREATE TABLE pipeline_version (
    id uuid NOT NULL,
    name text NOT NULL,
    pipeline uuid NOT NULL,
    code jsonb NOT NULL,
    files jsonb NOT NULL,
    CONSTRAINT "PK_pipeline_version" PRIMARY KEY (id),
    CONSTRAINT "FK_pipeline_version_pipeline_pipeline" FOREIGN KEY (pipeline) REFERENCES pipeline (id) ON DELETE CASCADE
);

CREATE TABLE application (
    id uuid NOT NULL,
    name text NOT NULL,
    pipeline uuid NULL,
    org uuid NULL,
    variables jsonb NOT NULL,
    source text NOT NULL,
    CONSTRAINT "PK_application" PRIMARY KEY (id),
    CONSTRAINT "FK_application_organization_org" FOREIGN KEY (org) REFERENCES organization (id) ON DELETE SET NULL,
    CONSTRAINT "FK_application_pipeline_version_pipeline" FOREIGN KEY (pipeline) REFERENCES pipeline_version (id) ON DELETE SET NULL
);

CREATE INDEX "IX_application_org" ON application (org);

CREATE INDEX "IX_application_pipeline" ON application (pipeline);

CREATE INDEX "IX_pipeline_org" ON pipeline (org);

CREATE INDEX "IX_pipeline_version_pipeline" ON pipeline_version (pipeline);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220618223103_Init', '6.0.4');

COMMIT;


