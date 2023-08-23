Yes I promise I'll get to writing/generating documentation soon. 

For now some quick notes:
- I have just merged it so you can switch back to the main branch.
- You have to start a1111 with `--deforum-api` to enable the API.
- You can then POST to `/deforum_api/batches`  to trigger a batch of runs, with a json payload like:
```
{
   "deforum_settings": { <content_of_your_settings_file> }
}
```

`"deforum_settings"` can also be an array of settings if you want to queue up multiple jobs in a single request. You can also include an optional  `"settings_overrides"`  to temporarily change global a1111 settings for the scope of a batch.  For example I use it to enable subtitles in tests here: https://github.com/deforum-art/sd-webui-deforum/blob/automatic1111-webui/tests/deforum_test.py

The response will include an array of job IDs that will be run as part of the batch.

- You can GET on `/deforum_api/batches` to see the list of known batches, GET on `/deforum_api/jobs`  to see all jobs, and GET `/deforum_api/jobs/<job_id>` to see the status of a given job. Explore the tests (linked above) for how you can use the response to monitor the status of a job.
- You can DELETE  on `/deforum_api/jobs/<job_id>` to cancel a queued job or interrupt an active job.

**Please consider the API beta for now: no one else has used it yet so who knows what I've missed, and it might change shape (feedback welcome).  **