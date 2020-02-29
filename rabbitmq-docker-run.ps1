docker run `
    --detach `
    --name rabbitmq-blog-management `
    --publish 5672:5672 `
    --publish 15672:15672 `
    rabbitmq:management