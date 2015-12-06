import 'bootstrap';
import {LogManager} from 'aurelia-framework';
import {ConsoleAppender} from 'aurelia-logging-console';

LogManager.addAppender(new ConsoleAppender());
LogManager.setLevel(LogManager.logLevel.warn);

export function configure(aurelia) {
    aurelia.use
        .defaultBindingLanguage()
        .defaultResources()
        .history()
        .router()
        .eventAggregator();

    aurelia.start().then(() => aurelia.setRoot('app', document.body));
}