# Deployer

Deployer is (will be) a full CI/CD platform built for governance (starting with CI).

## Philosophy

The biggest difference between this and other systems is the disconnect between pipelines
and code. In most CI systems the pipeline information exists as code in the REPO. This is
not the case for deployer.

The reason is to allow for more flexibility from a governance perspective. This also allows
teams to update and manage updates to pipelines across enterprises in a scalable fashion.

Imagine this flow.

1. System engineers need to migrate folks to different DAST. They
update the pipelines and push out a new version with the changes.
2. Team members who own pipelines are alerted to the new version
and given the option to upgrade (with rollback options in case of
issues.)
3. If team members do not upgrade system engineers can mark the old
versions as deprecated, which will visually warn anyone touching the
app with that version of the pipeline (as well as send another email)
4. If team members still do not upgrade system engineers can fully
disable the old version, which will stop any jobs using that version
from running.

## Org setup

There will be a definable taxonomy for organizing apps. The bottom most category will
be "organization", this will be the primary category for most resources. There will be
other levels of category that will place higher than organization and can be used for
grouping purposes.

The only resources available on categories other than organization are as follows.

* pipelines
* secrets

Secrets will fall down the stack to be available for pipelines. These
don't actually have to be secrets but any variables needed for pipelines
at a categorization level.

There will be an enterprise setting that will make ACLs apply to any category or app
underneath the category it's on. This will default to false.

## Pipelines

Pipelines will be YAML in structure and be managed either through the UI or the API
assuming that the appropriate ACLs are in place to allow editing.

They will be versioned and those versions will be kept in perpetuity (for now).

Pipelines will define variables that will need to be filled out for an app in order
for the jobs to run. Those variables can be defined in such a way to pull
them from org secrets instead of entry.

Pipelines will be triggered via VCS commits or tags.

If when attempting to upgrade a pipeline there is a change in input requirements
a screen will display asking for the new variables.

Jobs will be made of up steps.

When steps are run, there are three execution modes.

* direct  
  run directly on the nomad clients
* docker  
  run on docker installed on nomad clients (the most ideal)
* docker-remote  
  run on a remote docker cluster from the nomad client

From a resource scheduling perspective, running using docker on the nomad clients
is the most idea. Remote docker might be more convenient. If we platform entirely
on k8s we could just use a separate node group for running steps.

## ACLs

ACLs themselves will be OPA policies written in rego.

There will be ACLs for a few things. Organizations and categories will have two ACLs.
One for everything, and one that's optimized for basic access. This will allow for more
optimal permission checking across a large amount of orgs.

For these policies, you do not need to enumerate every possibility. The absence of a
permission will be an implicit denial.

Credentials will have their own ACLs, they will be a very basic policy that defines
whether a user can use the credential.

Here is the tentative list of permissions for orgs/categories.

* edit:acls
* edit:credentials
* edit:credential_acls
* edit:pipelines

You can also define custom permissions and ask for them in pipelines making it
pretty arbitrary.

You can also run OPA policies as a step, which will give you access to what's in the
file system at that point in time. These will effectively be pass/warn/fail policies.

There can also be fall down policies at the org/category level which will run at before
a pipeline is allowed to run. These will also act as pass/warn/fail policies.

There will also be a special world ACL that represents all orgs. This is for enterprise
wide permissions. This is intended for enterprise wide access. Likely this will be a
key/value in the database to allow team's to modify it directly in case of cluster
problems.

## Database

Running the database locally should be relatively easy. If you have  docker installed
you can use the `stack.yml` to start it up. Simply run `docker compose -f stack.yml up`
in the overview directory. Running the migrations should be from the same location
(after you have added `flyway` to your `PATH`) `flyway migrate`. If you need to clear
the database out you can use `flyway clean`. You will need to manually create the
`deployer` database but that should be all the manual effort needed.
