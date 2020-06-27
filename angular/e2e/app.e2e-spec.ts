import { JewelleryTemplatePage } from './app.po';

describe('Jewellery App', function() {
  let page: JewelleryTemplatePage;

  beforeEach(() => {
    page = new JewelleryTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
