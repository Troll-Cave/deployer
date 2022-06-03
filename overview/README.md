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
