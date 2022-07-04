export interface Application {
  id: string;
  name: string;
  pipeline: string;
  org: string;
  variables: Record<string, string>;
  source: string;
}
