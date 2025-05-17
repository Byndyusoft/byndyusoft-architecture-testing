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

<img src="assets/ExternalSystem.jpg" height="150">

## Documenting system elements

Elements of the system are given meaningful names and descriptions that answer the questions “what is it” and “why is it needed”.

### Example

```
storageWorker = container "Storage Worker" "Processes changes from queue" {
	...
}
```

<img src="assets/ElementDocumentation.jpg" height="150">
