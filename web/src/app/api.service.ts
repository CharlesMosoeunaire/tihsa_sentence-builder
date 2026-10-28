import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { ConfigService } from './config.service';
import { Sentence, WordType } from './models';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private readonly http = inject(HttpClient);
  private readonly config = inject(ConfigService);

  private url(path: string): string {
    return `${this.config.apiBaseUrl}${path}`;
  }

  getWordTypes() {
    return this.http.get<WordType[]>(this.url('/words'));
  }

  getSentences() {
    return this.http.get<Sentence[]>(this.url('/sentences'));
  }

  createSentence(wordIds: number[]) {
    return this.http.post<Sentence>(this.url('/sentences'), { wordIds });
  }

  updateSentence(id: number, wordIds: number[]) {
    return this.http.put<Sentence>(this.url(`/sentences/${id}`), { wordIds });
  }
}
