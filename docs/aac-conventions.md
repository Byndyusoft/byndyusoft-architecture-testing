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

## Linking system elements to code

Links to the corresponding repositories are added to the system elements. This will allow the user, both human and automated, to easily move from the abstractions of architecture to the reality of the code that implements it.

### Example

```
storageWorker = container "..." "..." {
    url "https://github.com/…"
	...
}
```

<img src="assets/RepositoryLink.jpg" height="150">

## Specifying the technologies of system elements

For each element of the system, the technologies used are specified. This will help the user understand what this or that element is based on, and the automation will improve the accuracy of the analysis.

### Example

```
storageWorker = container "..." "..." {
    technology ".NET Core"
    ...
}
```

<img src="assets/ElementTechnology.jpg" height="150">
