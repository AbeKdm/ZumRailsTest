import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms'; // Import FormsModule

interface Post {
  author: string;
  authorId: number;
  id: number;
  likes: number;
  popularity: number;
  reads: number;
  tags: string[];
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  public posts: Post[] = [];
  public tags: string = 'tech,health';
  public sortBy: string = 'id';
  public direction: string = 'asc';
  public error: string | null = null;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getPosts();
  }

  getPosts() {
    const url = `http://localhost:5292/api/Posts?tags=${this.tags}&sortBy=${this.sortBy}&direction=${this.direction}`;
    this.http.get<{ posts: Post[] }>(url).subscribe(
      (result) => {
        this.clearPosts();
        this.posts = result.posts;
        console.log(this.posts);
      },
      (errorResponse) => {
        this.clearPosts();
        console.error(errorResponse);
        if (errorResponse.error.errors) {
          // Handle errors array
          this.error = errorResponse.error.errors.join('. ');
        } else if (errorResponse.error.message) {
          // Handle single error object
          this.error = errorResponse.error.message;
        } else {
          this.error = 'An unexpected error occurred.';
        }
      }
    );
  }


  clearPosts() {
    this.posts = [];
    this.error = ''; // Reset error if request succeeds
  }

}
