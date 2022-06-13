import { processJobs } from "./process";

function wait(seconds: number): Promise<boolean> {
  return new Promise((resolve) => {
    setTimeout(() => {
      resolve(true);
    }, seconds * 1000);
  })
}

async function mainLoop(): Promise<void> {
  do {
    await processJobs();
  } while (await wait(5));
}

mainLoop();
