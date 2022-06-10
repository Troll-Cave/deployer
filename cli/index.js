/**
 * At some point I will make this an actual CLI but for now I just need
 * something to test with.
 */

// node index.js | pbcopy
// or the local equivalent to get the payload for postman
const fs = require('fs');

let yml = fs.readFileSync('./example.yml', {
  encoding: 'utf-8'
});

let body = {
  "name": "1",
  "pipeline": "0a3f9729-53d7-4bab-8b6b-4408b1e73a3a",
  "code": yml
};

console.log(JSON.stringify(body, null, '  '));
