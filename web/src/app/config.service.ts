import { Injectable } from '@angular/core';

interface AppConfig {
  apiBaseUrl: string;
}

/** Loads runtime settings from config.json so no environment value is compiled into the app. */
@Injectable({ providedIn: 'root' })
export class ConfigService {
  private config?: AppConfig;

  async load(): Promise<void> {
    const response = await fetch('config.json', { cache: 'no-store' });
    if (!response.ok) {
      throw new Error('Unable to load config.json');
    }
    this.config = (await response.json()) as AppConfig;
  }

  get apiBaseUrl(): string {
    if (!this.config) {
      throw new Error('Configuration has not been loaded.');
    }
    return this.config.apiBaseUrl.replace(/\/$/, '');
  }
}
