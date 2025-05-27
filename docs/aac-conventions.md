# Conventions for architecture code

## Explicit boundaries of the system

To explicitly indicate the boundaries of the designed system, all external systems are marked with a tag `ExternalSystem`. This makes it easy to recognize the designed system  on the diagram both visually and through automatic analysis.

### Example

```
model {
  pim = softwareSystem "PIM" {
      tags "ExternalSystem"
      ...
  }
}
views {
  ...
  styles {
    element "ExternalSystem" {
      background #7f7f7f
    }
  }
}
```

<img src="assets/ExternalSystemExample.jpg" height="150">

## Documenting system elements

Elements of the system are given meaningful names and descriptions that answer the questions “what is it” and “why is it needed”.

### Example

```
storageWorker = container "Storage Worker" "Processes changes from queue" {
	...
}
```

<img src="assets/ElementDocumentationExample.jpg" height="150">

## Linking system elements to code

Links to the corresponding repositories are added to the system elements. This will allow the user, both human and automated, to easily move from the abstractions of architecture to the reality of the code that implements it.

### Example

```
storageWorker = container "..." "..." {
    url "https://github.com/…"
    ...
}
```

<img src="assets/RepositoryLinkExample.jpg" height="150">

## Specifying the technologies of system elements

For each element of the system, the technologies used are specified. This will help the user understand what this or that element is based on, and the automation will improve the accuracy of the analysis.

### Example

```
storageWorker = container "..." "..." {
    technology ".NET Core"
    ...
}
```

<img src="assets/ElementTechnologyExample.jpg" height="150">

## Typing of system elements

System elements are typed using tags, for example:
- **Storage** for S3 or databases;
- **MQ** for queues;
- **Worker** for queue consumers;
- **CronJob** for scheduled jobs;
- **WebApi** for web APIs;
- **Frontend** for user interfaces.

This will help to visually separate the elements of architecture of different types, and the automation will improve the quality of analysis.

### Example

```
model {
  pim = softwareSystem "PIM" {
    s3 = container "..." "..." {
      tags "Storage" 
      ...
    }
  }
}
views {
  ...
  styles {
    element "Storage" {
      shape cylinder
    }
  }
}
```

<img src="assets/ElementTypeExample.jpg" height="150">

## Explicit declaring of message queues

Message queues are explicitly declared on the architecture using containers or untyped elements. This allows visual demonstration of who writes where, who reads from where, and at the same time readers and writers do not know about each other. In addition, due to the explicit declaration of queues, the code of automatic analyzers is simplified.

### Example

```
model {
  changesMQ = element "musicality_labs.data.changes" "RabbitMQ" {
    tags "MQ" 
  }
}
views {
  ...
  styles {
    element "MQ" {
      shape pipe
    }
  }
}
```

<img src="assets/MessageQueueExample.jpg" height="150">

## Marking of obsolete system elements

Obsolete but still active system elements are marked with a tag `Obsolete`. This allows the user to visually determine which elements are worth interacting with and which are not. In addition, the automatic analyzer will be able to calculate the metric of the number of uses of obsolete system elements.

### Example

```
model {
  pim = softwareSystem "PIM" {
    storageApi = container "..." "..."{
      tags "WebApi, Obsolete" 
	  ...
    }
  }
}
views {
  ...
  styles {
    element "Obsolete" {
      background #e0ecf2
    }
  }
}
```

<img src="assets/ObsoleteElementExample.jpg" height="150">

## Direction of relationships between system elements

In order to distinguish, both visually and by automatic analysis, which elements of the system depend on which, the relationships are directed according to the rules listed below.

1. If an element sends messages to a queue, then the relationship is directed from it to the queue.
<img src="assets/SendingMessageToQueueExample.jpg" height="80">

2. If an element receives messages from a queue, then the relationship is directed from the queue to it.
<img src="assets/ReceivingMessageFromQueueExample.jpg" height="80">

3. The relationship is directed from element A to element B if element A knows about element B and calls it.
<img src="assets/TwoElementsInteractionExample.jpg" height="80">

## Documenting relationships

For each relationship, a description of what is transmitted within it is added, which can be used to answer the question of why this relationship is needed.

### Example

```
changesMQ -> storageWorker "Changes" "..." "..."
```

<img src="assets/RelationshipDocumentationExample.jpg" height="150">

## Specifying the technologies of relationships

For each relationship, the technology used to organize it is specified. This will help the user understand what this or that relationship is based on, and the automation will improve the accuracy of the analysis.

### Example

```
changesMQ -> storageWorker "..." "AMQP" "..."
```

<img src="assets/RelationshipTechnologyExample.jpg" height="150">

## Typing of relationships

Using the `Sync` and `Async` tags, relationships are divided into synchronous and asynchronous. This will allow the user to quickly understand how the interaction of elements is organized, as well as improve the quality of automated analysis.

### Example

```
model {
  ...
  changesMQ -> storageWorker "..." "..." "Async"
}
views {
  ...
  styles {
    relationship "Async" {
      dashed true
    }
  }
}
```

<img src="assets/RelationshipTypeExample.jpg" height="150">

## Technical debts

Technical debts, when it comes to relationships, are marked using the `TechDebt` tag. This allows the user to visually track technical debts at the architecture level, and the automation to calculate the corresponding metrics.

### Example

```
model {
  ...
  storageWorker -> storageDatabase "Changes" "SQL" "Sync, TechDebt"
}
views {
  ...
  styles {
    relationship "TechDebt" {
      color red
    }
  }
}
```

<img src="assets/RelationshipTechnicalDebtExample.jpg" height="150">
