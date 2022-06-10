import { Injectable } from '@nestjs/common';
import * as fs from 'fs';
import * as jwt from 'jsonwebtoken';
import axios from 'axios';
import { GithubAccessToken, GithubInstallation } from '../input-models.interface';

// Mostly this is here to make it easier deal with
const githubVersionHeader = {
  Accept: 'application/vnd.github.v3+json',
};

// We'll want a config service later
const githubRoot = process.env.GITHUB_API_ROOT || 'https://api.github.com';
const pemLocation = process.env.PEM_LOCATION || './deployer-app.pem';

@Injectable()
export class GithubService {
  getGithubJWT(): string {
    return jwt.sign(
      {
        iss: 207885, // TODO: make this an ENV
        iat: Math.floor(Date.now() / 1000) - 60,
        exp: Math.floor(Date.now() / 1000) + 10 * 60,
      },
      fs.readFileSync(pemLocation), // TODO: make this location an ENV
      { algorithm: 'RS256' },
    );
  }

  async downloadZip(org: string, repo: string): Promise<void> {
    const token = this.getGithubJWT();

    const firstPartHeaders = {
      Authorization: `Bearer ${token}`,
      ...githubVersionHeader,
    };

    const ret = await axios.get<GithubInstallation[]>(`${githubRoot}/app/installations`, {
      headers: firstPartHeaders,
    });

    const installation = ret.data.filter((x) => x.target_type === 'Organization' && x.account.login === org)[0];

    const retToken = await axios.post<GithubAccessToken>(installation.access_tokens_url, null, {
      headers: firstPartHeaders,
    });

    const retData = await axios.get(`${githubRoot}/repos/${org}/${repo}/zipball`, {
      responseType: 'arraybuffer',
      headers: {
        Authorization: `token ${retToken.data.token}`,
      },
    });

    fs.writeFileSync('./lol.zip', retData.data, 'binary');

    return;
  }
}
