import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { firstValueFrom } from 'rxjs';
import { ApiService } from './api.service';
import { BuilderWord, Sentence, WordType } from './models';

@Component({
  selector: 'app-root',
  imports: [FormsModule, DatePipe],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App implements OnInit {
  private readonly api = inject(ApiService);

  readonly wordTypes = signal<WordType[]>([]);
  readonly sentences = signal<Sentence[]>([]);
  readonly selectedTypeId = signal<number | null>(null);
  readonly selectedWordId = signal<number | null>(null);
  readonly builder = signal<BuilderWord[]>([]);
  readonly editingId = signal<number | null>(null);
  readonly busy = signal(false);
  readonly error = signal<string | null>(null);
  readonly notice = signal<string | null>(null);

  readonly wordsForType = computed(
    () => this.wordTypes().find((t) => t.id === this.selectedTypeId())?.words ?? [],
  );
  readonly preview = computed(() => this.builder().map((w) => w.text).join(' '));

  async ngOnInit(): Promise<void> {
    try {
      const [types, sentences] = await Promise.all([
        firstValueFrom(this.api.getWordTypes()),
        firstValueFrom(this.api.getSentences()),
      ]);
      this.wordTypes.set(types);
      this.sentences.set(sentences);
    } catch (e) {
      this.error.set(this.describe(e));
    }
  }

  onTypeChange(id: number | null): void {
    this.selectedTypeId.set(id);
    this.selectedWordId.set(null);
  }

  addWord(): void {
    const type = this.wordTypes().find((t) => t.id === this.selectedTypeId());
    const word = type?.words.find((w) => w.id === this.selectedWordId());
    if (!type || !word) return;

    this.builder.update((list) => [...list, { wordId: word.id, text: word.text, typeName: type.name }]);
    this.selectedWordId.set(null);
    this.notice.set(null);
  }

  removeWord(index: number): void {
    this.builder.update((list) => list.filter((_, i) => i !== index));
  }

  move(index: number, direction: -1 | 1): void {
    this.builder.update((list) => {
      const target = index + direction;
      if (target < 0 || target >= list.length) return list;
      const copy = [...list];
      [copy[index], copy[target]] = [copy[target], copy[index]];
      return copy;
    });
  }

  async submit(): Promise<void> {
    if (this.builder().length === 0) return;

    this.busy.set(true);
    this.error.set(null);
    this.notice.set(null);

    const wordIds = this.builder().map((w) => w.wordId);
    const editingId = this.editingId();

    try {
      if (editingId === null) {
        await firstValueFrom(this.api.createSentence(wordIds));
      } else {
        await firstValueFrom(this.api.updateSentence(editingId, wordIds));
      }
      this.notice.set(editingId === null ? 'Sentence submitted.' : 'Sentence updated.');
      this.resetBuilder();
      this.sentences.set(await firstValueFrom(this.api.getSentences()));
    } catch (e) {
      this.error.set(this.describe(e));
    } finally {
      this.busy.set(false);
    }
  }

  edit(sentence: Sentence): void {
    this.builder.set(
      [...sentence.words]
        .sort((a, b) => a.position - b.position)
        .map((w) => ({ wordId: w.wordId, text: w.text, typeName: w.wordTypeName })),
    );
    this.editingId.set(sentence.id);
    this.error.set(null);
    this.notice.set(null);
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  cancelEdit(): void {
    this.resetBuilder();
    this.notice.set(null);
  }

  private resetBuilder(): void {
    this.builder.set([]);
    this.editingId.set(null);
    this.selectedTypeId.set(null);
    this.selectedWordId.set(null);
  }

  private describe(e: unknown): string {
    if (e instanceof HttpErrorResponse) {
      const problems: string[] = e.error?.errors ? Object.values<string[]>(e.error.errors).flat() : [];
      if (problems.length) return problems.join(' ');
      if (e.status === 0) return 'Cannot reach the server.';
    }
    return 'Something went wrong. Please try again.';
  }
}
