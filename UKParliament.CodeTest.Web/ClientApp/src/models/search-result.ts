export class SearchResult<T> {
  results: T[] = [];
  totalResults: number = 0;
  skip: number = 0;
  take: number = 0;
}
