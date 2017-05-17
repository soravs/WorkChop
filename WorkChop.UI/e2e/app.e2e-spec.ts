import { WorkchopPage } from './app.po';

describe('workchop App', () => {
  let page: WorkchopPage;

  beforeEach(() => {
    page = new WorkchopPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
