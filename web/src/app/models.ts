export interface Word {
  id: number;
  text: string;
}

export interface WordType {
  id: number;
  name: string;
  words: Word[];
}

export interface SentenceWord {
  wordId: number;
  text: string;
  wordTypeId: number;
  wordTypeName: string;
  position: number;
}

export interface Sentence {
  id: number;
  text: string;
  createdAt: string;
  updatedAt: string | null;
  words: SentenceWord[];
}

/** A word currently sitting in the sentence being built. */
export interface BuilderWord {
  wordId: number;
  text: string;
  typeName: string;
}
