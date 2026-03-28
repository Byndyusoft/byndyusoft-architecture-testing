workspace {
    
    model {
        
        changesMQ = element "musicality_labs.data.changes" "RabbitMQ" {
            tags "MQ" 
        }
        
        storage = softwareSystem "Storage" {
            storageWorker = container "Storage Worker" {
                tags "Worker" 
                technology ".NET Core"
                url "https://github.com/MusicalityLabs/musicality-labs-storage-worker"
            }
            storageApi = container "Storage Api" {
                tags "WebApi" 
                technology ".NET Core"
                url "https://github.com/MusicalityLabs/musicality-labs-storage-api"
                
                storageWorker -> this "Changes" "REST" "Sync"
            }
            storageDatabase = container "storageapi" {
                tags "Storage" 
                technology "PostgreSQL"
                
                storageApi -> this "Changes" "SQL" "Sync"
            }
        }
        
        changesMQ -> storageWorker "Changes" "AMQP" "Async"
    }
    
    views {
        systemLandscape {
            include * 
            autolayout lr
        }
        
        container storage {
            include *
            autolayout lr
        }
        
        styles {
            element "ExternalSystem" {
                background #7f7f7f
            }
            element "Storage" {
                shape cylinder
            }
            element "MQ" {
                shape pipe
            }
            relationship "Sync" {
                dashed false
            }
            relationship "Async" {
                dashed true
            }
            relationship "TechDebt" {
                color red
            }
        }
        
        theme default
    
    }
    
}