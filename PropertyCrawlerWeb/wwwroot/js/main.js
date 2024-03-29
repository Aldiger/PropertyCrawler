/* eslint-disable object-shorthand */

/* global Chart, CustomTooltips, getStyle, hexToRgba */

/**
 * --------------------------------------------------------------------------
 * CoreUI Free Boostrap Admin Template (v2.1.12): main.js
 * Licensed under MIT (https://coreui.io/license)
 * --------------------------------------------------------------------------
 */

/* eslint-disable no-magic-numbers */
// Disable the on-canvas tooltip





Chart.defaults.global.pointHitDetectionRadius = 1;
Chart.defaults.global.tooltips.enabled = false;
Chart.defaults.global.tooltips.mode = 'index';
Chart.defaults.global.tooltips.position = 'nearest';
Chart.defaults.global.tooltips.custom = CustomTooltips; // eslint-disable-next-line no-unused-vars

$(document).ready(function () {



    var cardChart1 = new Chart($('#card-chart1'), {
        type: 'bar',
        data: {
            labels: [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '],
            datasets: [{
                label: '',
                backgroundColor: 'rgba(255,255,255,.2)',
                borderColor: 'rgba(255,255,255,.55)',
                data: [87, 18, 8, 54, 43, 21, 4, 58, 56, 32, 21, 89, 43, 48, 76, 28]
            }]
        },
        options: {
            maintainAspectRatio: false,
            legend: {
                display: false
            },
            scales: {
                xAxes: [{
                    display: false,
                    barPercentage: 0.6
                }],
                yAxes: [{
                    display: false
                }]
            }
        }
    }); // eslint-disable-next-line no-unused-vars

    var cardChart2 = new Chart($('#card-chart2'), {
        type: 'bar',
        data: {
            labels: [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '],
            datasets: [{
                label: '',
                backgroundColor: 'rgba(255,255,255,.2)',
                borderColor: 'rgba(255,255,255,.55)',
                data: [78, 81, 80, 45, 34, 12, 40, 85, 65, 23, 12, 98, 34, 84, 67, 82]
            }]
        },
        options: {
            maintainAspectRatio: false,
            legend: {
                display: false
            },
            scales: {
                xAxes: [{
                    display: false,
                    barPercentage: 0.6
                }],
                yAxes: [{
                    display: false
                }]
            }
        }
    }); // eslint-disable-next-line no-unused-vars

    var cardChart3 = new Chart($('#card-chart3'), {
        type: 'bar',
        data: {
            labels: [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '],
            datasets: [{
                label: '',
                backgroundColor: 'rgba(255,255,255,.2)',
                borderColor: 'rgba(255,255,255,.55)',
                data: [48, 61, 50, 95, 24, 52, 50, 35, 95, 53, 42, 68, 34, 64, 27, 82]
            }]
        },
        options: {
            maintainAspectRatio: false,
            legend: {
                display: false
            },
            scales: {
                xAxes: [{
                    display: false,
                    barPercentage: 0.6
                }],
                yAxes: [{
                    display: false
                }]
            }
        }
    }); // eslint-disable-next-line no-unused-vars

    var cardChart4 = new Chart($('#card-chart4'), {
      type: 'bar',
      data: {
        labels: [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '],
        datasets: [{
          label: '',
          backgroundColor: 'rgba(255,255,255,.2)',
          borderColor: 'rgba(255,255,255,.55)',
          data: [78, 81, 80, 45, 34, 12, 40, 85, 65, 23, 12, 98, 34, 84, 67, 82]
        }]
      },
      options: {
        maintainAspectRatio: false,
        legend: {
          display: false
        },
        scales: {
          xAxes: [{
            display: false,
            barPercentage: 0.6
          }],
          yAxes: [{
            display: false
          }]
        }
      }
    }); // eslint-disable-next-line no-unused-vars

    var mainChart = new Chart($('#main-chart'), {
      type: 'line',
      data: {
        labels: ['M', 'T', 'W', 'T', 'F', 'S', 'S', 'M', 'T', 'W', 'T', 'F', 'S', 'S', 'M', 'T', 'W', 'T', 'F', 'S', 'S', 'M', 'T', 'W', 'T', 'F', 'S', 'S'],
        datasets: [{
          label: 'My First dataset',
          backgroundColor: hexToRgba(getStyle('--info'), 10),
          borderColor: getStyle('--info'),
          pointHoverBackgroundColor: '#fff',
          borderWidth: 2,
          data: [165, 180, 70, 69, 77, 57, 125, 165, 172, 91, 173, 138, 155, 89, 50, 161, 65, 163, 160, 103, 114, 185, 125, 196, 183, 64, 137, 95, 112, 175]
        }, {
          label: 'My Second dataset',
          backgroundColor: 'transparent',
          borderColor: getStyle('--success'),
          pointHoverBackgroundColor: '#fff',
          borderWidth: 2,
          data: [92, 97, 80, 100, 86, 97, 83, 98, 87, 98, 93, 83, 87, 98, 96, 84, 91, 97, 88, 86, 94, 86, 95, 91, 98, 91, 92, 80, 83, 82]
        }, {
          label: 'My Third dataset',
          backgroundColor: 'transparent',
          borderColor: getStyle('--danger'),
          pointHoverBackgroundColor: '#fff',
          borderWidth: 1,
          borderDash: [8, 5],
          data: [65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65]
        }]
      },
      options: {
        maintainAspectRatio: false,
        legend: {
          display: false
        },
        scales: {
          xAxes: [{
            gridLines: {
              drawOnChartArea: false
            }
          }],
          yAxes: [{
            ticks: {
              beginAtZero: true,
              maxTicksLimit: 5,
              stepSize: Math.ceil(250 / 5),
              max: 250
            }
          }]
        },
        elements: {
          point: {
            radius: 0,
            hitRadius: 10,
            hoverRadius: 4,
            hoverBorderWidth: 3
          }
        }
      }
    });
    var brandBoxChartLabels = ['January', 'February', 'March', 'April', 'May', 'June', 'July'];
    var brandBoxChartOptions = {
      responsive: true,
      maintainAspectRatio: false,
      legend: {
        display: false
      },
      scales: {
        xAxes: [{
          display: false
        }],
        yAxes: [{
          display: false
        }]
      },
      elements: {
        point: {
          radius: 0,
          hitRadius: 10,
          hoverRadius: 4,
          hoverBorderWidth: 3
        }
      } // eslint-disable-next-line no-unused-vars

    };
    var brandBoxChart1 = new Chart($('#social-box-chart-1'), {
      type: 'line',
      data: {
        labels: brandBoxChartLabels,
        datasets: [{
          label: 'My First dataset',
          backgroundColor: 'rgba(255,255,255,.1)',
          borderColor: 'rgba(255,255,255,.55)',
          pointHoverBackgroundColor: '#fff',
          borderWidth: 2,
          data: [65, 59, 84, 84, 51, 55, 40]
        }]
      },
      options: brandBoxChartOptions
    }); // eslint-disable-next-line no-unused-vars

    var brandBoxChart2 = new Chart($('#social-box-chart-2'), {
      type: 'line',
      data: {
        labels: brandBoxChartLabels,
        datasets: [{
          label: 'My First dataset',
          backgroundColor: 'rgba(255,255,255,.1)',
          borderColor: 'rgba(255,255,255,.55)',
          pointHoverBackgroundColor: '#fff',
          borderWidth: 2,
          data: [1, 13, 9, 17, 34, 41, 38]
        }]
      },
      options: brandBoxChartOptions
    }); // eslint-disable-next-line no-unused-vars

    var brandBoxChart3 = new Chart($('#social-box-chart-3'), {
      type: 'line',
      data: {
        labels: brandBoxChartLabels,
        datasets: [{
          label: 'My First dataset',
          backgroundColor: 'rgba(255,255,255,.1)',
          borderColor: 'rgba(255,255,255,.55)',
          pointHoverBackgroundColor: '#fff',
          borderWidth: 2,
          data: [78, 81, 80, 45, 34, 12, 40]
        }]
      },
      options: brandBoxChartOptions
    }); // eslint-disable-next-line no-unused-vars

    var brandBoxChart4 = new Chart($('#social-box-chart-4'), {
      type: 'line',
      data: {
        labels: brandBoxChartLabels,
        datasets: [{
          label: 'My First dataset',
          backgroundColor: 'rgba(255,255,255,.1)',
          borderColor: 'rgba(255,255,255,.55)',
          pointHoverBackgroundColor: '#fff',
          borderWidth: 2,
          data: [35, 23, 56, 22, 97, 23, 64]
        }]
      },
      options: brandBoxChartOptions
    });

});


//# sourceMappingURL=main.js.map