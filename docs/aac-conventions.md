# Conventions for architecture code

## Explicit boundaries of the system

To explicitly indicate the boundaries of the designed system, all external systems are marked with a tag `ExternalSystem`. This makes it easy to recognize the designed system  on the diagram both visually and through automatic analysis.

### Example
```
pim = softwareSystem "PIM" {
    tags "ExternalSystem"
    ...
}
```
