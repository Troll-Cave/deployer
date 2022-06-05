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
