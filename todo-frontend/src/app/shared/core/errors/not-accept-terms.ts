export class NotAcceptTermsError extends Error {
  token: string;

  constructor(token: string) {
    super('Você não aceitou os termos de uso.');
    this.name = 'Termos de Uso Não Aceito';
    this.token = token;
  }
}
