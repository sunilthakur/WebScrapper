import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NotesService } from '../services/notes.service';
import { jsPDF } from 'jspdf';
import domtoimage from 'dom-to-image';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  form: FormGroup;
  scrapeResponse: any;
  loading: boolean = false;
  showData: boolean = false;
  @ViewChild('scrapeResult', { static: false }) pdfTable: ElementRef;

  constructor(private fb: FormBuilder, private scrapeService: NotesService) {
    this.createForm();
  }

  ngOnInit() {
    this.showReportButton = false;
    this.loading = false;
  }
  createForm() {
    const reg = '(https?://)?([\\da-z.-]+)\\.([a-z.]{2,6})[/\\w .-]*/?';
    this.form = this.fb.group({
      s_url: ['', [Validators.required, Validators.pattern(reg)]],
    });
  }
  get s_url() { return this.form.get('s_url') };

  showReportButton: boolean;
  scrapeSite() {
    if (this.form.valid) {
      this.loading = true;
      this.showData = false;
      this.scrapeService.scrapeSite(this.form.value.s_url).subscribe(res => {
        this.loading = false;
        this.showData = true;
        if (res) {
          this.scrapeResponse = res;
          this.showReportButton = true;
        } else {

        }
      },
        error => {
          this.showReportButton = false;
          this.loading = false;
          this.showData = false;
          console.log("Error=>", error);
        });
    }
  }

  isReport: boolean;
  public downloadAsPDF() {
    var node = document.getElementById('scrapeResult');
    if (node) {
      node.innerHTML.replace("style='max-height:500px;overflow-y:auto'", "");
    }
    var img;
    var filename;
    var newImage;
    this.isReport = true;
    var that = this;
    domtoimage.toPng(node, { bgcolor: '#fff' })
      .then(function (dataUrl) {
        img = new Image();
        img.src = dataUrl;
        newImage = img.src;

        img.onload = function () {
          var pdfWidth = img.width;
          var pdfHeight = img.height;
          var doc;
          if (pdfWidth > pdfHeight) {
            doc = new jsPDF('l', 'px', [pdfWidth, pdfHeight]);
          } 
          else {
            doc = new jsPDF('p', 'px', [pdfWidth, pdfHeight]);
          }        
          var width = doc.internal.pageSize.getWidth();
          var height = doc.internal.pageSize.getHeight();
          doc.addImage(newImage, 'PNG', 10, 10, width, height);
          filename = 'scarpeReport_' + '.pdf';
          doc.save(filename);       
          that.isReport = false;
        };
      }.bind(this))
      .catch(function (error) {
        this.isReport = false;
        alert('Error occured while generating report, please try again');
      }.bind(this));
  }
}
