export class ValidationError extends Error {
  errors: string[];

  constructor(errors: string[]) {
    super('Erro de Validação');
    this.name = 'Erro de Validação';
    this.errors = errors;
  }
}
