<p align="center">
  <img height="200px" width="200px" style="text-align: center;" src="https://angular.io/assets/images/logos/angular/angular.svg">
  <h1 align="center">Lib-DigitalSignature</h1>
</p>




[![Support](https://img.shields.io/badge/Support-Angular%209%2B-blue.svg?style=flat-square)]()
[![License](https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square)]()
[![devDependency Status](https://img.shields.io/david/expressjs/express.svg?style=flat-square)]()

This library was generated with [Angular CLI](https://github.com/angular/angular-cli) version 9.1.12.

Use appropriate version based on your Angular version.

|  Angular 9  |
| ----------- |
| >=`v9.1.12` |


## Browser Support

| [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/chrome/chrome_48x48.png" alt="Chrome" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Chrome | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/firefox/firefox_48x48.png" alt="Firefox" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Firefox | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/edge/edge_48x48.png" alt="IE / Edge" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>IE / Edge | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/safari-ios/safari-ios_48x48.png" alt="iOS Safari" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Safari | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/opera/opera_48x48.png" alt="Opera" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Opera |
| ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Latest ✔                                                                                                                                                                                                      | Latest ✔                                                                                                                                                                                                          | IE11, Edge ✔                                                                                                                                                                                                    | Latest ✔                                                                                                                                                                                                                  | Latest ✔                                                                                                                                                                                                  |
## Requirements for quick use

| dependencia | version | link |
|----------|-------|--------|
| file-saver | 2.0.2 | [npm :link:](https://www.npmjs.com/package/file-saver/) |
| ngx-clipboard | 12.3.0| [npm :link:](https://www.npmjs.com/package/ngx-clipboard/) |

## Installation

`lib-digitalsignature` is available via [npm](https://www.npmjs.com/package/lib-digitalsignature) and [yarn](https://yarnpkg.com/package/lib-digitalsignature)

Using npm:

```bash
$ npm install lib-digitalsignature --save
```

Using yarn:

```bash
$ yarn add lib-digitalsignature
```

Using angular-cli:

```bash
$ ng add lib-digitalsignature
```

## Basic Usage

Import `NgxSpinnerModule` in in the root module(`AppModule`):

```typescript
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
// Import library module
import { DigitalSignatureModule } from "lib-digitalsignature";

@NgModule({
  imports: [
    // ...
    DigitalSignatureModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule {}
```

Add `DigitalSignatureModule` service wherever you want to use the `lib-digitalsignature`.

```typescript
import { DigitalSignatureModule } from "lib-digitalsignature";
```

Now use in your template

```html
<lib-digitalsignature></lib-digitalsignature>
```

## Advanced Usage - Service

<img src="https://img.icons8.com/officel/80/000000/road-worker.png"/>



## Build the project

Run `ng build lib-digitalsignature` to build the project. The build artifacts will be stored in the `dist/` directory.


## Creator

#### [Patricio E. Arena](mailto:patricio.e.arena@gmail.com)

- [@GitHub](https://github.com/patricioarena)

## Future Plan

- Smaller bundle

## License

lib-digitalsignature is [MIT licensed](./LICENSE).