using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet.Utils
{
    public class StatusChecker
    {
        public static string Check(int? status)
        {
            string result = "<span class='label'>Не определен</span>";

            switch (status)
            {
                case 0: result = "<span class='label myTooltip' title='Заявка отправлена и на текущий момент не подписана руководителем. Как только руководитель подпишет заявку, статус измениться на \"Подписана\"'>Опубликована</span>";
                    break;

                case 1: result = "<span class='label label-warning myTooltip' title='Заявка подписана руководителем и на текущий момент не обработана диспетчером. Как только диспетчер обработает заявку, статус измениться на \"Принята\"'>Подписана</span>";
                    break;

                case 2: result = "<span class='label label-success myTooltip' title='Диспетчер принял эту заявку, подробности можно узнать перейдя в режим просмотра заявки'>Принята</span>";
                    break;

                case 3: result = "<span class='label label-important myTooltip' title='Заявка отклонена, подробности можно узнать перейдя в режим просмотра заявки'>Возвращена</span>";
                    break;

                case 4: result = "<span class='label label-important myTooltip'>Отменена</span>";
                    break;

                case 5: result = "<span class='label label-success myTooltip'>В работе</span>";
                    break;

                case 6: result = "<span class='label label-success myTooltip'>Выполнена</span>";
                    break;

                default: result = "<span class='label myTooltip'>Не определен</span>";
                    break;
            }

            return result;
        }
    }
}