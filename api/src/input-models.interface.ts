import { exhaustiveTypeException } from "tsconfig-paths/lib/try-path";

export interface PipelineInput {
  name: string;
}

export interface PipelineModel {
  id: string;
  name: string;
  organization: string;
}

export interface PipelineVersionModel {
  id?: string;
  name: string;
  pipeline: string; // UUID
  variables?: any[]; // JSON structure, pull from code
  code: string;
}

export interface GithubInstallation {
  id: string;
  access_tokens_url: string;
  target_type: string;
  account: GithubInstallationAccount;
}

export interface GithubInstallationAccount {
  login: string;
}

export interface GithubAccessToken {
  token: string;
}
