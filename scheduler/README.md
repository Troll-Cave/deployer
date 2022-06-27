# Scheduler

This is the meat and bones of running jobs. It requires a bit of set up.

1. Sit your `github.pem` at the root of the scheduler folder.
2. Set your github settings via the below exports.

```
export githubApiRoot="{your github api root}"
export githubAppId="{your githuhb app id}"
```

You can likely make the exports easier in your IDE. Otherwise just run it in the shell you run the app.
