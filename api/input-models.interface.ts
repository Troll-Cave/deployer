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
  files: Record<string, string>;
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

/************* Pipeline stuff goes here *************/

export interface Pipeline {
  variables?: PipelineVariable[];
  steps: PipelineStepCommand[];
  flow: PipelineFlow[];

  files: {
    name: string;
    can_replace?: boolean;
    binary?: boolean;
    location: string;
  }[];
}

export interface PipelineVariable {
  name: string;
  type: 'string' | 'credential';
  scope: 'org' | 'local';
  secret: boolean;
}

// Don't use this directly, inherit it and add to union on pipeline interface
export interface PipelineStep {
  name: string;
  persist: boolean;
}

export interface PipelineStepCommand extends PipelineStep {
  type: 'commands';
  locals: {
    name: string;
    type: 'string';
  };
  actions: {
    name: string;
    command: string;
  }[];
}

export interface PipelineFlow {
  name?: string;
  step: string;
  depends_on: string[];
  locals: Record<string, string>;
}

export interface OrgModel {
  id?: string;
  name: string;
}

export interface AppInput {
  id?: string;
  name: string;
  org: string;
  source: string;
  variables: Record<string, string>;
}

export interface StartPipelineInput {
  id: string; // id of the application
}
